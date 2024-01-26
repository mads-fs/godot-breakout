using Godot;
using System;

namespace Game.GameData
{
    /// <summary>A Pellet data structure which is converted into a game representation at runtime when needed.</summary>
    public readonly struct Pellet
    {
        public readonly int Points { get { return points; } }
        private readonly int points = 1;
        public readonly Color @Color { get { return color; } }
        private readonly Color color = new(1f, 1f, 1f, 1f);

        public Pellet(int points, Color color)
        {
            this.points = points;
            this.color = color;
        }

        public override string ToString() => $"[{points}]";
        /// <summary>Convenience structure for when we need no Pellet but still need data.</summary>
        public readonly static Pellet Empty = new(0, new Color(0f, 0f, 0f, 0f));

        public static bool operator ==(Pellet p1, Pellet p2) => (p1.points == p2.points && p1.color == p2.color);
        public static bool operator !=(Pellet p1, Pellet p2) => (p1.points != p2.points && p1.color != p2.color);

        public readonly override int GetHashCode() => HashCode.Combine(points, color);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Pellet pellet)
            {
                return pellet == this;
            }
            return false;
        }
    }
}
