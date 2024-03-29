﻿using Godot;

namespace Game.Nodes
{
    public partial class Paddle : CharacterBody2D
    {
        [Export] private float speed = 275f;

        private Vector2 startPosition;
        private bool isAcceptingInput;
        private float velocity = 0f;

        public float GetVelocity() => velocity;

        public override void _EnterTree()
        {
            this.startPosition = this.GlobalPosition;
            GameEvents.OnLevelEnd += this.HandleOnLevelEnd;
            GameEvents.OnPlayerDeath += this.HandleOnPlayerDeath;
        }

        public override void _Process(double delta)
        {
            float horizontal = 0f;
            if (GameManager.Instance.Lives > 0)
            {
                if (Input.IsActionJustPressed("fire")) GameEvents.BroadcastOnBallRelease();
                if (Input.IsActionPressed("move_left")) horizontal = Vector2.Left.X;
                if (Input.IsActionPressed("move_right")) horizontal = Vector2.Right.X;
            }

            float velocityDelta = 0f;
            if (horizontal < 0f)
            {
                velocityDelta = (Mathf.Abs(horizontal) * speed * (float)delta) * -1f;
            }
            else if (horizontal > 0f)
            {
                velocityDelta = horizontal * speed * (float)delta;
            }
            this.velocity = Mathf.Clamp(velocityDelta, -1, 1f);
            this.MoveAndCollide(new(velocityDelta, 0f));
        }

        private void HandleOnLevelEnd(int newLevelIndex) => this.GlobalPosition = this.startPosition;
        private void HandleOnPlayerDeath() => this.HandleOnLevelEnd(0);
    }
}
