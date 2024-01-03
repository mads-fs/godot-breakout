using Game.Nodes;

namespace Game
{
    public class ScoreManager
    {
        public int Score { get { return score; } }
        private int score;

        public ScoreManager()
        {
            GameEvents.OnPelletHit += HandleOnPelletHit;
            GameEvents.OnGameRestart += HandleOnGameRestart;
        }

        public void AddScore(int score)
        {
            this.score += score;
            GameEvents.BroadcastOnScoreChange(this.score);
        }

        private void HandleOnPelletHit(PelletNode pellet) => AddScore(pellet.Score);
        private void HandleOnGameRestart() => score = 0;
    }
}
