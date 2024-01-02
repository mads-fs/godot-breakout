using System.Text;

namespace Game.GameData
{
    public class PelletMap
    {
        public Pellet[,] Map { get { return map; } }
        private readonly Pellet[,] map;

        public PelletMap(int height, int width)
        {
            map = new Pellet[height, width];
        }

        public void Add(int x, int y, Pellet pellet) => map[x, y] = pellet;

        public override string ToString()
        {
            StringBuilder sb = new();
            for(int x = 0; x < map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    sb.Append(map[x, y]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
