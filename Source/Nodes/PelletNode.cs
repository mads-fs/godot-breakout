using Godot;

namespace Game.Nodes
{
	public partial class PelletNode : Area2D
	{
		public int Score { get { return score; } }
		private int score;
		private Sprite2D sprite;

		public override void _EnterTree()
		{
			sprite = GetNode<Sprite2D>("Sprite2D");
		}

		public void Initialize(int score, Color color) { this.SetScore(score); this.SetColor(color); }
		public void SetScore(int score) => this.score = score;
		public void SetColor(Color color) => sprite.Modulate = color;
	}
}
