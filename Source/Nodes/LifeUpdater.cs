using Godot;
using System.Text;

namespace Game.Nodes
{
    /// <summary>Used on a text component to update the Life display text automatically.</summary>
    public partial class LifeUpdater : RichTextLabel
    {
        public override void _EnterTree()
        {
            GameEvents.OnPlayerDeath += HandleOnPlayerDeath;
        }
        public override void _ExitTree()
        {
            GameEvents.OnPlayerDeath -= HandleOnPlayerDeath;
        }
        public override void _Ready() => HandleOnPlayerDeath();

        private void HandleOnPlayerDeath()
        {
            StringBuilder sb = new();
            int counter = 0;
            while (counter < GameManager.Instance.Lives)
            {
                sb.Append('*');
                counter++;
            }
            this.Text = $"{sb}";
        }

        private void HandleOnGameRestart() => HandleOnPlayerDeath();
    }
}
