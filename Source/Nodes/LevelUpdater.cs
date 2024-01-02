using Godot;

namespace Game.Nodes
{
    public partial class LevelUpdater : Node
    {
        [Export] private RichTextLabel Label;

        public override void _EnterTree() => GameEvents.OnLevelEnd += HandleOnLevelEnd;
        public override void _ExitTree() => GameEvents.OnLevelEnd -= HandleOnLevelEnd;

        private void HandleOnLevelEnd(int newLevelIndex) => Label.Text = $"{newLevelIndex + 1}";
    }
}
