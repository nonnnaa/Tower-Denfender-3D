using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public enum GameState
    {
        None,
        PAUSE,
        PLAYING
    }
    private GameState currentGameState = GameState.None;
    public GameState CurrentGameState => currentGameState;

    public void ChangeGameState(GameState newGameState)
    {
        currentGameState = newGameState;
    }
}
