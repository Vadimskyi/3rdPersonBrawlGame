using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

        private void Awake()
        {
            _currentPlayer = Instantiate(_playerPrefab, _initialPlayerPos, Quaternion.identity, transform);
            _currentPlayer.gameObject.SetActive(true);
            _cameraToFolow.Follow = _currentPlayer.GetComponentInChildren<Animator>().transform;
            _cameraToFolow.LookAt = _currentPlayer.GetComponentInChildren<Animator>().transform;
        }

        public void RespawnPlayer()
        {
            _currentPlayer.transform.position = _initialPlayerPos;
        }
    }
}