using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class PlayerDash : MonoBehaviour
    {
        public event Action OnDash = delegate { };

        private User _user;
        private Animator _animator;
        private Transform _transform;
        private GameSettings _settings;
        private PlayerMovement _playerMovement;

        private float _timeFromLastDash = 0;

        public PlayerDash(
            User user,
            Transform target,
            Animator animator,
            PlayerMovement playerMovement)
        {
            _user = user;
            _transform = target;
            _animator = animator;
            _playerMovement = playerMovement;
            _settings = Services.Get<GameSettings>();
        }


        public void CustomUpdate()
        {
            if (!_user.Character.IsDead)
            {
                ReadInput();
            }
        }

        public void CustomFixedUpdate()
        {
            _timeFromLastDash += Time.fixedDeltaTime;
        }

        private void ReadInput()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
            {
                TryDash();
            }
#endif
        }

        public async void TryDash()
        {
            if (_timeFromLastDash < _settings.DefaultCharacterSettings.DashRate
                || !_playerMovement.CanMove) return;
            _timeFromLastDash = 0;
            OnDash?.Invoke();
            _playerMovement.BlockMovement(true);
            await RemoteDash();
            _playerMovement.BlockMovement(false);
        }

        public async Task RemoteDash()
        {
            _animator.SetBool("dashing", true);
            await _playerMovement.Dash();
            _animator.SetBool("dashing", false);
        }
    }
}