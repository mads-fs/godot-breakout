#pragma warning disable CA2211 // Non-constant fields should not be visible

using Game.Nodes;
using System;

namespace Game
{
    /// <summary>
    /// General GameEvents class that allows any part of the code to know about
    /// any game event without needing to know the underlying sender of the event.
    /// </summary>
    public static class GameEvents
    {
        /// <summary>Fires when the player releases the ball from the Paddle.</summary>
        public static event Action OnBallRelease = delegate { };
        /// <summary>Fires when the ball collides with anything.</summary>
        public static event Action OnBallCollide = delegate { };
        /// <summary>Fires when a Pellet is hit and includes what Pellet was hit.</summary>
        public static event Action<PelletNode> OnPelletHit = delegate { };
        /// <summary>Fires when the scores changes.</summary>
        public static event Action<int> OnScoreChange = delegate { };
        /// <summary>Fires when a Level is beat.</summary>
        public static event Action<int> OnLevelEnd = delegate { };
        /// <summary>Fires when the game ends.</summary>
        public static event Action OnGameEnd = delegate { };
        /// <summary>Fires when the Player dies.</summary>
        public static event Action OnPlayerDeath = delegate { };

        public static void BroadcastOnBallRelease() => OnBallRelease.Invoke();
        public static void BroadcastOnBallCollide() => OnBallCollide.Invoke();
        public static void BroadcastOnPelletHit(PelletNode pellet) => OnPelletHit.Invoke(pellet);
        public static void BroadcastOnScoreChange(int score) => OnScoreChange.Invoke(score);
        public static void BroadcastOnLevelEnd(int newLevelIndex) => OnLevelEnd.Invoke(newLevelIndex);
        public static void BroadcastOnGameEnd() => OnGameEnd.Invoke();
        public static void BroadcastOnPlayerDeath() => OnPlayerDeath.Invoke();
    }
}
