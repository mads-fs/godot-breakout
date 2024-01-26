using Godot;

namespace Game.Nodes
{
    /// <summary>Used on a text component to update the Level display text automatically.</summary>
    public partial class LevelUpdater : RichTextLabel
    {
        public override void _EnterTree() => GameEvents.OnLevelEnd += HandleOnLevelEnd;
        public override void _ExitTree() => GameEvents.OnLevelEnd -= HandleOnLevelEnd;

        private void HandleOnLevelEnd(int newLevelIndex) => this.Text = $"{newLevelIndex + 1}";
    }
}