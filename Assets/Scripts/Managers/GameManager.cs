using System;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static Action<GameState> OnBeforeGameStateChanged;
    public static Action<GameState> OnAfterGameStateChanged;

    public GameState State { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start() => ChangeState(GameState.Start);

    public void ChangeState(GameState newState)
    {
        OnBeforeGameStateChanged?.Invoke(newState);

        State = newState;

        switch (newState)
        {
            case GameState.Start:
                HandleStart();
                break;
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.End:
                HandleEnd();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnAfterGameStateChanged?.Invoke(newState);
        Debug.Log($"New state: {newState}");
    }

    void HandleStart()
    {
        UniverseManager.instance.StartGenerate();
        ChangeState(GameState.StartGame);
    }

    void HandleStartGame()
    {

    }

    void HandleEnd()
    {
        UniverseManager.instance.StopGenerate();
    }
}

public enum GameState
{
    Start,
    StartGame,
    End
}
