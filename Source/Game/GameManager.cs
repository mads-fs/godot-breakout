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
        [Export] private AudioStreamPlayer2D audioPlayer;
        [Export] private AudioStreamWav impactSound;
        [Export] private AudioStreamWav deathSound;

        public int Lives { get { return lives; } }
        private int lives;

        private int levelIndex = -1;
        private readonly List<PelletMap> levels = new();

        private int pelletsHit = 0;
        private int pelletsInLevel = 0;

        private bool readInput = false;

        public override void _EnterTree()
        {
            // Instantiating the GameManager as a singleton
            if (GameManager.Instance != null) Free();
            else
            {
                instance = this;
                this.endScreenNode.Visible = false;
                this.lives = this.playerLives;
                // We subscribe if the instance is valid
                GameEvents.OnPelletHit += HandleOnPelletHit;
                GameEvents.OnBallCollide += HandleOnBallCollide;
                GameEvents.OnPlayerDeath += HandleOnPlayerDeath;
                GameEvents.OnGameEnd += HandleOnGameEnd;
            }
        }

        public override void _ExitTree()
        {
            // In the event of an instance carrying over between scenes
            // we unsubscribe here to not subscribe multiple times to the same events.
            GameEvents.OnPelletHit -= HandleOnPelletHit;
            GameEvents.OnPlayerDeath -= HandleOnPlayerDeath;
            GameEvents.OnGameEnd -= HandleOnGameEnd;
        }

        public override void _Ready()
        {
            this.readInput = true;
            this.GenerateLevels();
            this.NextLevel();
        }

        public override void _Process(double delta)
        {
            if (this.readInput && Input.IsActionJustPressed("clear_level"))
            {
                this.ScoreManager.AddScore(this.pelletsInLevel);
                this.pelletsHit = this.pelletsInLevel - 1;
                this.HandleOnPelletHit(null);
            }
        }

        /// <summary>Generate all levels from the pixel data found in the included PNG files ahead of time.</summary>
        private void GenerateLevels()
        {
            DirAccess access = DirAccess.Open(levelFolderPath);
            string[] files = access.GetFiles();
            foreach (string file in files)
            {
                // We remove '.import' because Godot adds the extension when Exporting
                // the game for Windows.
                string cleanFile = file.Replace(".import", "");
                string fileExt = cleanFile.Replace(".import", "").Split('.').Last().ToLower();
                if (fileExt.Contains("png") && file.ToLower().Contains("level"))
                {
                    CompressedTexture2D texture = GD.Load<CompressedTexture2D>($"{levelFolderPath}/{cleanFile}");
                    GD.Print(texture);
                    Image image = texture.GetImage();
                    int height = image.GetHeight();
                    int width = image.GetWidth();
                    PelletMap map = new(height, width);
                    for (int x = 0; x < height; x++)
                    {
                        for (int y = 0; y < width; y++)
                        {
                            Color color = image.GetPixel(y, x);
                            // If alpha is zero, we consider it an empty pellet
                            Pellet pellet = color.A == 0 ? Pellet.Empty : new(1, color);
                            map.Add(x, y, pellet);
                        }
                    }
                    this.levels.Add(map);
                }
            }
        }

        /// <summary>Advances the Level Index by 1 and evaluates whether to continue the game or end it.</summary>
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

        /// <summary>
        /// Run at the start of every new round of the game to setup the current level previously generated with <see cref="GenerateLevels"/>
        /// </summary>
        private void SetupLevel()
        {
            // Free up existing Pellets on Level Clear.
            for (int index = pelletParent.GetChildCount() - 1; index > -1; index--)
            {
                pelletParent.GetChild(index, true).QueueFree();
            }
            this.pelletsInLevel = 0;
            this.pelletsHit = 0;

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
                    this.pelletsInLevel += 1;
                }
            }
        }

        private void HandleOnPelletHit(PelletNode pellet)
        {
            this.pelletsHit += 1;
            if (this.pelletsHit >= this.pelletsInLevel)
            {
                this.NextLevel();
            }
        }

        private void HandleOnBallCollide()
        {
            audioPlayer.Stop();
            audioPlayer.Stream = impactSound;
            audioPlayer.Play();
        }

        private void HandleOnGameRestart()
        {
            this.levelIndex = -1;
            this.pelletsHit = 0;
            this.lives = playerLives;
            this.pelletParent.Visible = true;
            this.endScreenNode.Visible = false;
            this.NextLevel();
        }

        private void HandleOnPlayerDeath()
        {
            audioPlayer.Stop();
            audioPlayer.Stream = deathSound;
            audioPlayer.Play();
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
            this.readInput = false;
        }
    }
}