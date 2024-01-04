using Godot;

namespace Game.Nodes
{
    public partial class EndZone : Area2D
    {
        public override void _EnterTree()
        {
            this.BodyShapeEntered += (rid, body, bodyIndex, shapeIndex) => GameEvents.BroadcastOnPlayerDeath();
        }
    }
}
