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
            GameEvents.onCharacterRotated += GameEvents_onCharacterRotated;
            GameEvents.onCharacterFiredGun += GameEvents_onCharacterFireGun;
            GameEvents.onCharacterTakenDamage += GameEvents_onCharacterTakenDamage;
            GameEvents.onCharacterRespawned += GameEvents_onCharacterRespawned;
            GameEvents.onCharacterPushed += GameEvents_onCharacterPushed;
            GameEvents.onCharacterKick += GameEvents_onCharacterKick;
        }

        public void RespawnPlayer()
        {
            NetworkEvents.RespawnCharacter(UserData.Instance.User.Id);
        }

        private void CreatePlayer(User user)
        {
            _initialPlayerPos = user.Character.Position.Value;
            var view = Instantiate(_playerPrefab, _initialPlayerPos, user.Character.Rotation, transform);
            view.gameObject.SetActive(true);

            var player = new PlayerFacade(user, view, _moveProjectiles, _spawnProjectiles);
            _activePlayers.Add(user.Id, player);
            SubscribeToAllPlayers(player);
            if (user.IsLocalUser)
            {
                _cameraToFolow.Follow = view.Animator.transform;
                _cameraToFolow.LookAt = view.Animator.transform;
                CurrentPlayer = player;
                SubscribeToMainPlayer(player);
            }
            else
            {
                view.Rigidbody.isKinematic = true;
            }
        }

        private void Update()
        {
            CurrentPlayer?.CustomUpdate();
        }

        private void FixedUpdate()
        {
            foreach (var playerFacade in _activePlayers.Values)
            {
                playerFacade.CustomFixedUpdate();
            }
            //CurrentPlayer.CustomFixedUpdate();
        }

        private void SubscribeToAllPlayers(PlayerFacade facade)
        {
            Debug.Log("SubscribeToPlayer - " + facade.User.Name);
            facade.Health.OnTakeDamage += (damage, user) =>
            {
                NetworkEvents.CharacterTakeDamage(new UserTakeDamage { UserId = user.Id, Damage = damage });
            };
            facade.OnPushed += (direction) =>
            {
                NetworkEvents.CharacterPush(new UserPush
                {
                    UserId = UserData.Instance.User.Id,
                    TargetId = facade.User.Id,
                    Direction = direction
                });
            };
        }

        private void SubscribeToMainPlayer(PlayerFacade facade)
        {
            facade.Shooting.OnFire += () =>
            {
                NetworkEvents.CharacterFireGun(new UserShot
                {
                    UserId = facade.User.Id,
                    From = facade.View.FirePoint.position,
                    Direction = facade.View.FirePoint.forward
                });
            };

            facade.Movement.OnMove += (target, movement) =>
            {
                NetworkEvents.CharacterMoved(movement);
            };
            facade.Movement.OnRotate += (angle) =>
            {
                if (facade.User.Character.IsDead) return;
                NetworkEvents.CharacterRotated(new UserRotation { UserId = facade.User.Id, Angle = angle });
            };
            facade.Kick.OnKick += () =>
            {
                NetworkEvents.CharacterKick(facade.User.Id);
            };
        }

        private void GameEvents_onCharacterFireGun(UserShot data)
        {
            if (!_activePlayers.ContainsKey(data.UserId)) return;
            _activePlayers[data.UserId].Shooting.FireGun(data);
        }

        private void GameEvents_onCharacterTakenDamage(UserTakeDamage data)
        {
            if (!_activePlayers.ContainsKey(data.UserId)) return;
            _activePlayers[data.UserId].Health.TakeDamage(data.Damage);
        }

        private void GameEvents_onCharacterMoved(UserMovement data)
        {
            if (!_activePlayers.ContainsKey(data.UserId)) return;
            _activePlayers[data.UserId].Movement.MoveAndRotate(data);
        }

        private void GameEvents_onCharacterRotated(UserRotation data)
        {
            if (!_activePlayers.ContainsKey(data.UserId)) return;
            //_activePlayers[data.UserId].Movement.Rotate(data.Angle);
        }

        private void GameEvents_onCharacterPushed(UserPush data)
        {
            if (!_activePlayers.ContainsKey(data.TargetId)) return;
            _activePlayers[data.TargetId].PushEventReceived(data);
        }

        private void GameEvents_onCharacterKick(int userId)
        {
            if (!_activePlayers.ContainsKey(userId)) return;
            _activePlayers[userId].Kick.RemoteKick();
        }

        private void GameEvents_onCharacterRespawned(PlayerCharacterModel data)
        {
            if (!_activePlayers.ContainsKey(data.UserId)) throw new ArgumentException($"No character with userId \"{data.UserId}\" was found!");

            _activePlayers[data.UserId].Movement.ResetToDefault(data);
        }
    }
}