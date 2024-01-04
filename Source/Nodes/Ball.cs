using Godot;

namespace Game.Nodes
{
    public partial class Ball : CharacterBody2D
    {
        [Export] private float initialSpeed = 200f;
        [Export] private float speedUpFactor = 1.05f;

        private float speed;
        private Vector2 startPosition;
        private Vector2 direction = Vector2.Up;
        private Vector2 velocityIncrease = Vector2.Zero;
        private bool isFollowingPaddle = true;
        private CharacterBody2D paddle;

        public override void _EnterTree()
        {
            this.startPosition = this.GlobalPosition;
            GameEvents.OnBallRelease += this.HandleOnBallRelease;
            GameEvents.OnLevelEnd += this.HandleOnLevelEnd;
            GameEvents.OnGameEnd += this.HandleOnGameEnd;
            GameEvents.OnPlayerDeath += this.HandleOnPlayerDeath;
        }

        public override void _Ready()
        {
            speed = initialSpeed;
            paddle = GetParent().GetNode<CharacterBody2D>("Paddle");
        }

        public override void _Process(double delta)
        {
            if (this.isFollowingPaddle)
            {
                this.GlobalPosition = new(this.paddle.GlobalPosition.X, this.startPosition.Y);
            }
            else
            {
                this.Velocity = this.direction * this.speed * (float)delta;
                if (this.MoveAndCollide(this.Velocity) is KinematicCollision2D collision)
                {
                    this.direction = -this.direction.Reflect(collision.GetNormal());
                    if (collision.GetCollider() is Paddle)
                    {
                        this.speed *= this.speedUpFactor;
                    }
                    else if (collision.GetCollider() is PelletNode pellet)
                    {
                        GameEvents.BroadcastOnPelletHit(pellet);
                        pellet.QueueFree();
                    }
                }
            }
        }

        private void HandleOnBallRelease()
        {
            this.isFollowingPaddle = false;
            this.direction = Vector2.Up;
            this.velocityIncrease = new(0f, -this.velocityIncrease.Y);
        }

        private void HandleOnLevelEnd(int levelIndex)
        {
            this.isFollowingPaddle = true;
            this.direction = Vector2.Up;
            this.velocityIncrease = Vector2.Zero;
        }

        private void HandleOnGameEnd()
        {
            this.isFollowingPaddle = true;
        }

        private void HandleOnPlayerDeath() => this.HandleOnLevelEnd(0);
    }
}