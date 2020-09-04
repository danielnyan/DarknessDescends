﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDG
{
    public class GameManager : Manager<GameManager>
    {
        public enum GameState
        {
            Pregame,
            Running,
            Paused,
            Postgame
        }
        public GameState CurrentGameState { get; set; } = GameState.Pregame;

        public override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                UpdateState(GameState.Running);
            };
        }

        void Update()
        {
            if (CurrentGameState == GameState.Pregame)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }

        private void UpdateState(GameState state)
        {
            GameState previousGameState = CurrentGameState;
            CurrentGameState = state;

            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (CurrentGameState)
            {
                case GameState.Pregame:
                    Time.timeScale = 1.0f;
                    break;
                case GameState.Running:
                    Time.timeScale = 1.0f;
                    break;
                case GameState.Paused:
                    Time.timeScale = 0.0f;
                    break;
            }
            EventManager.Instance.Raise(new GameStateChangedEvent
            {
                currentGameState = CurrentGameState,
                previousGameState = previousGameState
            }
            );
        }

        public void TogglePause()
        {
            UpdateState(CurrentGameState == GameState.Running ?
                GameState.Paused : GameState.Running);
        }

        public void RestartGame()
        {
            UpdateState(GameState.Pregame);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            UpdateState(GameState.Running);
        }
    }

    public class GameStateChangedEvent : GameEvent
    {
        public GameManager.GameState currentGameState;
        public GameManager.GameState previousGameState;
    }
}