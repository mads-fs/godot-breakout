namespace Game
{
    public class ScoreManager
    {
        public int Score { get { return score; } }
        private int score;

        public void AddScore(int score)
        {
            this.score += score;
            GameEvents.BroadcastOnScoreChange(this.score);
        }
    }
}
