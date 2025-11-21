using UnityEngine;
using UnityEngine.Events;
using System;

public static class GameEvents
{
    public static event Action<int> OnScoreChanged;
    public static event Action OnLevelCleared;
    public static event Action OnBallLost;
    public static event Action OnPauseGame;
    public static event Action<IPowerUp> OnPowerUpSpawned;
    public static event Action OnGameStarted;
    public static event Action OnGameOver;

    public static void RaiseScoreChanged(int newScore) => OnScoreChanged?.Invoke(newScore);
    public static void RaiseLevelCleared() => OnLevelCleared?.Invoke();
    public static void RaiseBallLost() => OnBallLost?.Invoke();
    public static void RaisePause() => OnPauseGame?.Invoke();
    public static void RaisePowerUpSpawned(IPowerUp powerUp) => OnPowerUpSpawned?.Invoke(powerUp);
    public static void RaiseGameOver() => OnGameOver?.Invoke();
    public static void RaiseGameStarted() => OnGameStarted?.Invoke();


}