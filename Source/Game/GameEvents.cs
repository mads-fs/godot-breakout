#pragma warning disable CA2211 // Non-constant fields should not be visible

using System;

namespace Game
{
    public static class GameEvents
    {
        public static Action<int> OnScoreChange = delegate { };
        public static Action<int> OnLevelEnd = delegate { };
        public static Action OnGamEnd = delegate { };

        public static void BroadcastOnScoreChange(int score) => OnScoreChange.Invoke(score);
        public static void BroadcastOnLevelEnd(int newLevelIndex) => OnLevelEnd.Invoke(newLevelIndex);
        public static void BroadcastOnGameEnd() => OnGamEnd.Invoke();
    }
}
