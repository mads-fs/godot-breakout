using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Nodes
{
    public partial class LifeUpdater : RichTextLabel
    {
        public override void _EnterTree()
        {
            GameEvents.OnPlayerDeath += HandleOnPlayerDeath;
            GameEvents.OnGameRestart += HandleOnGameRestart;
        }
        public override void _ExitTree()
        {
            GameEvents.OnPlayerDeath -= HandleOnPlayerDeath;
            GameEvents.OnGameRestart -= HandleOnGameRestart;
        }
        public override void _Ready() => HandleOnPlayerDeath();

        private void HandleOnPlayerDeath()
        {
            StringBuilder sb = new();
            int counter = 0;
            while(counter < GameManager.Instance.Lives)
            {
                sb.Append('*');
                counter++;
            }
            this.Text = $"{sb}";
        }

        private void HandleOnGameRestart() => HandleOnPlayerDeath();
    }
}
