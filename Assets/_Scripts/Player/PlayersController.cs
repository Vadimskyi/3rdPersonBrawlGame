using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayersController : MonoSingleton<PlayersController>
    {
        public PlayerFacade CurrentPlayer { get; private set; }

        [SerializeField]
        private PlayerView _playerPrefab;
        [SerializeField]
        private Vector3 _initialPlayerPos;
        [SerializeField]
        private CinemachineVirtualCamera _cameraToFolow;
        [SerializeField]
        private UiCanvasController _canvas;

        private MoveProjectiles _moveProjectiles;
        private SpawnProjectiles _spawnProjectiles;

        private Dictionary<int, PlayerFacade> _activePlayers;

        private void Awake()
        {
            _moveProjectiles = GetComponent<MoveProjectiles>();
            _spawnProjectiles = GetComponent<SpawnProjectiles>();
            _activePlayers = new Dictionary<int, PlayerFacade>();
            CombatRoomData.Instance.Users.ForEach(CreatePlayer);
            CombatRoomData.Instance.OnNewUserJoined += CreatePlayer;
            GameEvents.onCharacterMoved += GameEvents_onCharacterMoved;
            GameEvents.onCharacterRotated += GameEvents_onCharacterRotated; ;
        }

        public void RespawnPlayer()
        {
            CurrentPlayer.View.transform.position = _initialPlayerPos;
        }

        private void CreatePlayer(User user)
        {
            _initialPlayerPos = user.Character.Position;
            var view = Instantiate(_playerPrefab, _initialPlayerPos, user.Character.Rotation, transform);
            view.gameObject.SetActive(true);

            var player = new PlayerFacade(user, view, _moveProjectiles, _spawnProjectiles);
            _activePlayers.Add(user.Id, player);
            if (!user.IsMainPlayer) return;

            _cameraToFolow.Follow = view.Animator.transform;
            _cameraToFolow.LookAt = view.Animator.transform;
            CurrentPlayer = player;
            SubscribeToPlayer(CurrentPlayer);
        }

        private void Update()
        {
            CurrentPlayer?.CustomUpdate();
        }

        private void FixedUpdate()
        {
            CurrentPlayer.CustomFixedUpdate();
        }

        private void SubscribeToPlayer(PlayerFacade facade)
        {
            facade.Health.OnDamageTaken += damage => { _canvas.PlayerHealthOnDamageTaken(damage);};
            facade.Shooting.OnFire += () => { facade.Shooting.FireGun();};
            facade.Movement.OnMove += (target, movement) => { NetworkEvents.CharacterMoved(new UserMovement{UserId = UserData.Instance.User.Id, Direction =  movement}); _canvas.OnPlayerMove(target, movement); };
            facade.Movement.OnRotate += (angle) => { NetworkEvents.CharacterRotated(new UserRotation { UserId = UserData.Instance.User.Id, Angle = angle}); };
            //facade.Movement.OnMove += (target, movement) => { facade.Movement.Move(movement); _canvas.OnPlayerMove(target, movement); };
            //facade.Movement.OnRotate += (angle) => { facade.Movement.Rotate(angle); };
        }

        private void GameEvents_onCharacterMoved(UserMovement data)
        {
            if (!_activePlayers.ContainsKey(data.UserId)) return;
            _activePlayers[data.UserId].Movement.Move(data.Direction);
            //if (UserData.Instance.User.Id.Equals(data.UserId))
             //   _canvas.OnPlayerMove(_activePlayers[data.UserId].View.transform, data.Direction);
        }

        private void GameEvents_onCharacterRotated(UserRotation data)
        {
            if (!_activePlayers.ContainsKey(data.UserId)) return;
            _activePlayers[data.UserId].Movement.Rotate(data.Angle);
        }
    }
}