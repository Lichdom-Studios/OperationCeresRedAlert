using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    SETUP,
    READY,
    COUNTDOWN,
    PLAY,
    GAMEOVER
}

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static event Action<GameState> OnGameStateChange;

    GameState state;
    void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.SETUP:
                break;
            case GameState.READY:
                break;
            case GameState.COUNTDOWN:
                break;
            case GameState.PLAY:
                break;
            case GameState.GAMEOVER:
                break;
            default:
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }

    public GameState GetGameState()
    {
        return state;
    }
}
