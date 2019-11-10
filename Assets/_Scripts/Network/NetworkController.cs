using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO;
using UnityEngine;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class NetworkController
    {
        private GameSettings _settings;
        private SocketManager _manager;
        private DefaultGameRouter _router;
        private GameStateController _gameStateController;

        public NetworkController(GameStateController gameStateController, GameSettings gameSettings)
        {
            _settings = gameSettings;
            _gameStateController = gameStateController;
            _gameStateController.OnStateChanged += OnStateChanged;
            OpenConnection();
        }

        private void OpenConnection()
        {
            SocketOptions options = new SocketOptions();
            options.AutoConnect = false;
            options.Timeout = new TimeSpan(0, 0, 5, 0);

            _manager = new SocketManager(new Uri("http://95.179.129.130:1111/socket.io/"), options);
            //_manager = new SocketManager(new Uri("http://192.168.0.104:1111/socket.io/"), options);
/*#if UNITY_EDITOR
            _manager = new SocketManager(new Uri("http://95.179.129.130:1111/socket.io/"), options);
#else
            _manager = new SocketManager(new Uri("http://95.179.129.130:1111/socket.io/"), options);
#endif*/
            _manager.Encoder = new CustomJsonDecoder();
            _manager.Open();
            _router = new DefaultGameRouter(_manager, _settings.NetworkSettings);
        }

        private void OnStateChanged(IState currentState)
        {
            if (!(currentState is GameState)) return;
            var gState = (GameState)currentState;
            switch (gState.GameStateType)
            {
                case GameStateType.Networking:
                    break;
            }
        }
    }
}