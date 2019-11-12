using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class PlayerKick
    {
        public event Action OnKick = delegate { };

        private User _user;
        private Animator _animator;
        private Transform _transform;
        private Collider _kickCollider;
        private Rigidbody _rigidbody;
        private GameSettings _settings;
        private PlayerMovement _playerMovement;
        private IDisposable _triggerDetectionTask;

        private float _timeFromLastKick = 0;
        private bool _kickDetected = false;

        public PlayerKick(
            User user,
            Transform target,
            Collider kickCollider,
            Animator animator,
            Rigidbody rigidbody,
            PlayerMovement playerMovement)
        {
            _user = user;
            _transform = target;
            _animator = animator;
            _rigidbody = rigidbody;
            _kickCollider = kickCollider;
            _playerMovement = playerMovement;
            _settings = Services.Get<GameSettings>();
            _kickCollider.enabled = false;
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
            _timeFromLastKick += Time.fixedDeltaTime;
        }

        private void ReadInput()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
            {
                TryKick();
            }
#endif
        }

        public async void TryKick()
        {
            if (_timeFromLastKick < _settings.DefaultCharacterSettings.KickRate 
                || !_playerMovement.CanMove) return;
            _timeFromLastKick = 0;
            OnKick?.Invoke();
            StartHitEnemy();
            await RemoteKick();
            EndHitEnemy();
        }

        public async Task RemoteKick()
        {
            _animator.SetBool("kick", true);
            await Observable.Timer(
                TimeSpan.FromMilliseconds(_settings.AnimationSetting.KickAnimationTime));
            _animator.SetBool("kick", false);
        }

        private void StartHitEnemy()
        {
            _playerMovement.BlockMovement(true);
            _kickCollider.enabled = true;
            _triggerDetectionTask = _kickCollider.OnTriggerEnterAsObservable().Subscribe(target =>
            {
                var view = target.gameObject.GetComponent<PlayerView>();
                if (view == null) return;
                _kickDetected = true;
                view.Facade.Pushed(_transform.forward);
            });
        }

        private void EndHitEnemy()
        {
            _playerMovement.BlockMovement(false);
            _triggerDetectionTask?.Dispose();
            _kickCollider.enabled = false;
            _kickDetected = false;
        }
    }
}