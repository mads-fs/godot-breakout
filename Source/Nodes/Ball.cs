using Godot;

namespace Game.Nodes
{
    public partial class Ball : CharacterBody2D
    {
        [Export] private float speed = 100f;
        [Export] private float verticalVelocityIncrease = 0.05f;

        private Vector2 direction = Vector2.Up;
        private Vector2 velocityIncrease = Vector2.Zero;

        public override void _EnterTree()
        {
            GameEvents.OnLevelEnd += HandleOnLevelEnd;
        }

        public override void _Ready()
        {
            this.velocityIncrease = Vector2.One;
        }

        public override void _Process(double delta)
        {
            this.Velocity = this.direction * this.velocityIncrease * speed * (float)delta;
            KinematicCollision2D collision = this.MoveAndCollide(this.Velocity);
            if (collision != null)
            {
                Vector2 hitNormal = collision.GetNormal();
                this.direction = -this.Velocity.Reflect(hitNormal).Normalized();
                if (collision.GetCollider() is Paddle paddle)
                {
                    float x = paddle.GetVelocity();
                    float y = 0f;
                    if (this.Velocity.Y < 0) y = this.Velocity.Y + -this.verticalVelocityIncrease;
                    else if (this.Velocity.Y > 0) y = this.Velocity.Y + this.verticalVelocityIncrease;
                    this.velocityIncrease = new(this.velocityIncrease.X + x, y);
                    GD.Print(this.velocityIncrease);
                }
                else if (collision.GetCollider() is PelletNode pellet)
                {
                    GameManager.Instance.ScoreManager.AddScore(pellet.Score);
                    pellet.QueueFree();
                    GameEvents.BroadcastOnPelletHit();
                }
            }
        }

        private void HandleOnLevelEnd(int levelIndex)
        {
            // Place Ball correctly for the next game
        }
    }
}