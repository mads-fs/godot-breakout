using Game.GameData;
using Game.Nodes;
using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public partial class GameManager : Node
    {
        public static GameManager Instance { get { return instance; } }
        private static GameManager instance;
        public ScoreManager ScoreManager { get { return scoreManager; } }
        private readonly ScoreManager scoreManager = new();

        [Export] private string levelFolderPath = "res://Levels";
        [Export] private PackedScene pelletNodeScene;
        [Export] private Node pelletParent;
        [Export] private Vector2 firstPelletPosition;
        [Export] private float pelletXOffset;
        [Export] private float pelletYOffset;

        private int levelIndex = -1;
        private readonly List<PelletMap> levels = new();

        public override void _EnterTree()
        {
            if (GameManager.Instance != null) Free();
            else instance = this;
        }

        public override void _Ready()
        {
            GenerateLevels();
            NextLevel();
        }

        private void GenerateLevels()
        {
            DirAccess access = DirAccess.Open(levelFolderPath);
            string[] files = access.GetFiles();
            Color noColor = new(0f, 0f, 0f, 0f);
            foreach (string file in files)
            {
                string fileExt = file.Split('.').Last().ToLower();
                if (fileExt.Contains("png") && file.ToLower().Contains("level"))
                {
                    CompressedTexture2D texture = GD.Load<CompressedTexture2D>($"{levelFolderPath}/{file}");
                    Image image = texture.GetImage();
                    int height = image.GetHeight();
                    int width = image.GetWidth();
                    PelletMap map = new(height, width);
                    for (int x = 0; x < height; x++)
                    {
                        for (int y = 0; y < width; y++)
                        {
                            Color color = image.GetPixel(y, x);
                            Pellet pellet = color == noColor ? Pellet.Empty : new(1, color);
                            map.Add(x, y, pellet);
                        }
                    }
                    levels.Add(map);
                }
            }
        }

        private void NextLevel()
        {
            levelIndex += 1;
            if (levelIndex < levels.Count)
            {
                GameEvents.BroadcastOnLevelEnd(levelIndex);
                SetupLevel();
            }
            else
            {
                GameEvents.BroadcastOnGameEnd();
            }
        }

        private void SetupLevel()
        {
            PelletMap map = levels[this.levelIndex];
            for (int x = 0; x < map.Map.GetLength(0); x++)
            {
                for (int y = 0; y < map.Map.GetLength(1); y++)
                {
                    Pellet data = map.Map[x, y];
                    if (data == Pellet.Empty) continue;
                    Vector2 offset = new(pelletYOffset * y, pelletXOffset * x);

                    PelletNode pNode = (PelletNode)pelletNodeScene.Instantiate();
                    pelletParent.AddChild(pNode);

                    pNode.GlobalPosition = firstPelletPosition;
                    pNode.Initialize(data.Points, data.Color);
                    pNode.Translate(offset);
                }
            }
        }
    }
}