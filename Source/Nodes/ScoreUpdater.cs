using Godot;

namespace Game.Nodes
{
    /// <summary>Used on a text component to update the Score display text automatically.</summary>
    public partial class ScoreUpdater : RichTextLabel
    {
        public override void _EnterTree() => GameEvents.OnScoreChange += HandleOnScoreEvent;
        public override void _ExitTree() => GameEvents.OnScoreChange -= HandleOnScoreEvent;

        private void HandleOnScoreEvent(int score) => this.Text = $"{score}";
    }
}