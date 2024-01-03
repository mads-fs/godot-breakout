#pragma warning disable CA2211 // Non-constant fields should not be visible

using System;

namespace Game
{
    public static class GameEvents
    {
        public static Action OnPelletHit = delegate { };
        public static Action<int> OnScoreChange = delegate { };
        public static Action<int> OnLevelEnd = delegate { };

        public static void BroadcastOnPelletHit() => OnPelletHit.Invoke();
        public static void BroadcastOnScoreChange(int score) => OnScoreChange.Invoke(score);
        public static void BroadcastOnLevelEnd(int newLevelIndex) => OnLevelEnd.Invoke(newLevelIndex);
    }
}
