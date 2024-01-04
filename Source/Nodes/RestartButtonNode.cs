using Godot;

namespace Game.Nodes
{
    public partial class RestartButtonNode : TextureButton
    {
        public override void _Pressed() => GameEvents.BroadcastOnGameRestart();
    }
}