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
        [Export] private Node2D pelletParent;
        [Export] private Vector2 firstPelletPosition;
        [Export] private float pelletXOffset;
        [Export] private float pelletYOffset;
        [Export] private Node2D endScreenNode;
        [Export] private int playerLives = 3;

        public int Lives { get { return lives; } }
        private int lives;

        private int levelIndex = -1;
        private readonly List<PelletMap> levels = new();

        public override void _EnterTree()
        {
            if (GameManager.Instance != null) Free();
            else
            {
                instance = this;
                this.endScreenNode.Visible = false;
                this.lives = this.playerLives;
                GameEvents.OnPelletHit += HandleOnPelletHit;
                GameEvents.OnGameRestart += HandleOnGameRestart;
                GameEvents.OnPlayerDeath += HandleOnPlayerDeath;
                GameEvents.OnGameEnd += HandleOnGameEnd;
            }
        }

        public override void _ExitTree()
        {
            GameEvents.OnPelletHit -= HandleOnPelletHit;
            GameEvents.OnGameRestart -= HandleOnGameRestart;
            GameEvents.OnPlayerDeath -= HandleOnPlayerDeath;
            GameEvents.OnGameEnd -= HandleOnGameEnd;
        }

        public override void _Ready()
        {
            this.GenerateLevels();
            this.NextLevel();
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
                    this.levels.Add(map);
                }
            }
        }

        private void NextLevel()
        {
            this.levelIndex += 1;
            if (this.levelIndex < this.levels.Count)
            {
                GameEvents.BroadcastOnLevelEnd(this.levelIndex);
                this.SetupLevel();
            }
            else
            {
                GameEvents.BroadcastOnGameEnd();
            }
        }

        private void SetupLevel()
        {
            PelletMap map = this.levels[this.levelIndex];
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

        private void HandleOnPelletHit(PelletNode pellet)
        {
            if (this.pelletParent.GetChildCount() == 0)
            {
                this.NextLevel();
            }
        }

        private void HandleOnGameRestart()
        {
            this.levelIndex = -1;
            this.lives = playerLives;
            this.pelletParent.Visible = true;
            this.endScreenNode.Visible = false;
            this.NextLevel();
        }

        private void HandleOnPlayerDeath()
        {
            if (this.lives - 1 <= 0)
            {
                GameEvents.BroadcastOnGameEnd();
            }
            else this.lives -= 1;
        }

        private void HandleOnGameEnd()
        {
            this.lives = 0;
            this.pelletParent.Visible = false;
            this.endScreenNode.Visible = true;
        }
    }
}