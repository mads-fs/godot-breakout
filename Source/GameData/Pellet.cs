#pragma warning disable CA2211 // Non-constant fields should not be visible

using Godot;
using System;

namespace Game.GameData
{
    public struct Pellet
    {
        public int Points { get { return points; } }
        private int points = 1;
        public Color @Color { get { return color; } }
        private Color color = new(1f, 1f, 1f, 1f);

        public Pellet(int points, Color color)
        {
            this.points = points;
            this.color = color;
        }

        public override string ToString() => $"[{points}]";
        public readonly static Pellet Empty = new(0, new Color(0f, 0f, 0f, 0f));

        public static bool operator ==(Pellet p1, Pellet p2) => (p1.points == p2.points && p1.color == p2.color);
        public static bool operator !=(Pellet p1, Pellet p2) => (p1.points != p2.points && p1.color != p2.color);

        public override int GetHashCode() => HashCode.Combine(points, color);

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
