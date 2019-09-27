using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerController : MonoSingleton<PlayerController>
    {
        public PlayerFacade CurrentPlayer
        {
            get { return _currentPlayer; }
        }

        [SerializeField]
        private PlayerFacade _playerPrefab;
        [SerializeField]
        private Vector3 _initialPlayerPos;
        [SerializeField]
        private CinemachineVirtualCamera _cameraToFolow;

        private PlayerFacade _currentPlayer;

        private User _user;

        private void Awake()
        {
            NetworkEvents.onUserDataReceived += OnUserDataReceived;
        }

        private void OnUserDataReceived(User user)
        {
            _user = user;
            _initialPlayerPos = user.Character.Position;
            _currentPlayer = Instantiate(_playerPrefab, _initialPlayerPos, Quaternion.identity, transform);
            _currentPlayer.gameObject.SetActive(true);
            _cameraToFolow.Follow = _currentPlayer.GetComponentInChildren<Animator>().transform;
            _cameraToFolow.LookAt = _currentPlayer.GetComponentInChildren<Animator>().transform;
        }

        private void Start()
        {
            NetworkEvents.JoinGame("Vadimskyi");
        }

        public void RespawnPlayer()
        {
            _currentPlayer.transform.position = _initialPlayerPos;
        }
    }
}