using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class CompositionRoot : MonoBehaviour
    {
        private GameController _gameController;
        private GameStateFactory _gameStateFactory;
        private NetworkController _networkController;
        private GameStateController _gameStateController;
        private GameLevelController _gameLevelController;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _gameLevelController = GetComponent<GameLevelController>();
            InstallControllers();
        }

        private void InstallControllers()
        {
            //Initialize Dependencies
            _gameStateFactory = new GameStateFactory();
            _gameStateController = new GameStateController(_gameStateFactory);
            _gameStateController.SetState(GameStateType.Enter);
            _networkController = new NetworkController(_gameStateController);


            //Start Game
            _gameController = new GameController(_gameLevelController, _gameStateController);
        }
    }
}
