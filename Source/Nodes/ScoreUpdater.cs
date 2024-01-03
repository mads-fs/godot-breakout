using Godot;

namespace Game.Nodes
{
    public partial class ScoreUpdater : RichTextLabel
    {
        public override void _EnterTree() => GameEvents.OnScoreChange += HandleOnScoreEvent;
        public override void _ExitTree() => GameEvents.OnScoreChange -= HandleOnScoreEvent;

        private void HandleOnScoreEvent(int score) => this.Text = $"{score}";
    }
}