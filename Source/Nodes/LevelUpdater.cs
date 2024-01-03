using Godot;

namespace Game.Nodes
{
    public partial class LevelUpdater : RichTextLabel
    {
        public override void _EnterTree() => GameEvents.OnLevelEnd += HandleOnLevelEnd;
        public override void _ExitTree() => GameEvents.OnLevelEnd -= HandleOnLevelEnd;

        private void HandleOnLevelEnd(int newLevelIndex) => this.Text = $"{newLevelIndex + 1}";
    }
}