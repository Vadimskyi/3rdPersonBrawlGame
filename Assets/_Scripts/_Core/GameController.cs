using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class GameController
    {
        private GameLevelController _gameLevelController;
        private GameStateController _gameStateController;

        private IDisposable _initializer;

        public GameController(
            GameLevelController gameLevelController,
            GameStateController gameStateController)
        {
            _gameLevelController = gameLevelController;
            _gameStateController = gameStateController;
            ListenToEvents();
        }

        private void OnUserPressPlayButton()
        {
            //load user data
            NetworkEvents.JoinGame(_gameLevelController.NameField.text);
            //
        }

        private void OnStateChanged(IState currentState)
        {
            if (!(currentState is GameState)) return;
            var gState = (GameState) currentState;
            switch (gState.GameStateType)
            {
                case GameStateType.Networking:
                    break;
            }
        }

        private void ListenToEvents()
        {
            _gameStateController.OnStateChanged += OnStateChanged;
            _gameLevelController.PlayButton.OnClickAsObservable().Subscribe(_ => OnUserPressPlayButton());
            Observable.FromMicroCoroutine(WaitInitialization).Subscribe((_) => { }, OnInitializationCompleted);
        }

        private void OnInitializationCompleted()
        {
            Debug.Log("OnInitializationCompleted");
            _gameLevelController.LoadArena();
        }

        private IEnumerator WaitInitialization()
        {
            while (!UserData.Instance.IsInitialized || !CombatRoomData.Instance.IsInitialized)
            {
                yield return null;
            }
        }
    }
}
