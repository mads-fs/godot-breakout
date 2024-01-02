using Godot;

namespace Game.Nodes
{
    public partial class ScoreUpdater : Node
    {
        [Export] private RichTextLabel Label;

        public override void _EnterTree() => GameEvents.OnScoreChange += HandleOnScoreEvent;
        public override void _ExitTree() => GameEvents.OnScoreChange -= HandleOnScoreEvent;

        private void HandleOnScoreEvent(int score) => Label.Text = $"{score}";
    }
}