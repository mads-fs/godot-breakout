#pragma warning disable CA2211 // Non-constant fields should not be visible

using Game.Nodes;
using System;

namespace Game
{
    public static class GameEvents
    {
        public static Action OnBallRelease = delegate { };
        public static Action<PelletNode> OnPelletHit = delegate { };
        public static Action<int> OnScoreChange = delegate { };
        public static Action OnGameRestart = delegate { };
        public static Action<int> OnLevelEnd = delegate { };
        public static Action OnGameEnd = delegate { };
        public static Action OnPlayerDeath = delegate { };

        public static void BroadcastOnBallRelease() => OnBallRelease.Invoke();
        public static void BroadcastOnPelletHit(PelletNode pellet) => OnPelletHit.Invoke(pellet);
        public static void BroadcastOnScoreChange(int score) => OnScoreChange.Invoke(score);
        public static void BroadcastOnGameRestart() => OnGameRestart.Invoke();
        public static void BroadcastOnLevelEnd(int newLevelIndex) => OnLevelEnd.Invoke(newLevelIndex);
        public static void BroadcastOnGameEnd() => OnGameEnd.Invoke();
        public static void BroadcastOnPlayerDeath() => OnPlayerDeath.Invoke();
    }
}
