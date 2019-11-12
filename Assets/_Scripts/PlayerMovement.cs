using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class PlayerMovement : IDisposable
    {
        public event Action<Transform, UserMovement> OnMove = delegate { };
        public event Action<Quaternion> OnRotate = delegate { };

        public bool CanMove => !_user.Character.IsDead && !_blockMovement;

        [SerializeField]
        private float _moveSpeed = 4;
        [SerializeField]
        private float _turnSpeed = 10;

        private float _fixedDeltaTime = 0.03f;

        private User _user;
        private Animator _animator;
        private Transform transform;
        private Rigidbody _rigidbody;
        private GameSettings _settings;
        private CharacterController _characterController;

        private Vector3 _prevMovementForce;
        private Vector3 _movementForce;
        private IDisposable _movementTask;
        private IDisposable _stumbleTask;
        private bool _blockMovement;

        private bool _canSendNetworkMovement;
        private float _networkSendRate = 10;
        private float _timeBetweenMovementEnd;
        private float _timeBetweenMovementStart;

        [Header("Lerping Properties")]
        private bool _isLerpingPosition;
        private bool _isLerpingRotation;
        private Vector3 _realPosition;
        private Quaternion _realRotation;
        private Vector3 _lastRealPosition;
        private Quaternion _lastRealRotation;
        private float _timeStartedLerping;
        private float _timeToLerp;
        private float _lastYPosition;

        public PlayerMovement(
            User user,
            Transform target,
            Animator animator,
            Rigidbody rigidbody,
            CharacterController characterController)
        {
            _user = user;
            transform = target;
            _animator = animator;
            _rigidbody = rigidbody;
            _characterController = characterController;
            _settings = Services.Get<GameSettings>();

            _blockMovement = false;
            _canSendNetworkMovement = true;
            _realPosition = target.position;
            _realRotation = target.rotation;
            user.Character.OnReset.Subscribe(model =>
            {
                transform.position = model.Position.Value;
                transform.rotation = model.Rotation;
            });

            _lastYPosition = transform.position.y;
            _movementTask = transform.ObserveEveryValueChanged(t => t.position.y).Subscribe(UpdateYPosition);
        }

        /// <summary>
        /// 3rd person movement
        /// </summary>
        public void CustomUpdate()
        {
            if (CanMove)
            {
                ReadInput();
            }
            else if(!_blockMovement)
            {
                _movementForce = Vector3.zero;
            }
        }

        public void CustomFixedUpdate()
        {
            if (!_user.IsLocalUser)
            {
                NetworkLerp();
                return;
            }

            if (_movementForce.magnitude.Equals(0) && _movementForce == _prevMovementForce) return;

            MoveLocal(_movementForce);
            if (_movementForce.magnitude > 0)
            {
                Quaternion newDirection = Quaternion.LookRotation(_movementForce);
                RotateLocal(newDirection);
            }

            _prevMovementForce = _movementForce;

            if (_canSendNetworkMovement)
            {
                _canSendNetworkMovement = false;
                StartSendingMove();
            }
        }

        public async Task StartSendingMove()
        {
            _timeBetweenMovementStart = Time.time;
            await Observable.Timer(TimeSpan.FromSeconds(1 / _networkSendRate));
            _timeBetweenMovementEnd = Time.time;
            OnMove?.Invoke(transform, new UserMovement
            {
                Position = transform.position,
                Angle =  transform.rotation,
                Time = _timeBetweenMovementEnd - _timeBetweenMovementStart,
                UserId = _user.Id
            });
            _canSendNetworkMovement = true;
        }

        //on remote client
        public void MoveAndRotate(UserMovement newMovement)
        {
            //_animator.SetFloat("moveSpeed", (transform.position - newMovement.Position).magnitude);
            _lastRealPosition = _realPosition;
            _lastRealRotation = _realRotation;
            _realPosition = newMovement.Position;
            _realRotation = newMovement.Angle;
            _timeToLerp = newMovement.Time;

            if (_realPosition != transform.position)
            {
                _isLerpingPosition = true;
            }

            if (_realRotation.eulerAngles != transform.rotation.eulerAngles)
            {
                _isLerpingRotation = true;
            }

            _timeStartedLerping = Time.time;
        }

        //on local client
        public void MoveLocal(Vector3 movementForce)
        {
            var newPos = transform.position + movementForce * _fixedDeltaTime * _moveSpeed;
            _rigidbody.position = new Vector3(newPos.x, transform.position.y, newPos.z);
            _user.Character.Position.SetValueAndForceNotify(_rigidbody.position);
            _animator.SetFloat("moveSpeed", movementForce.magnitude);
        }

        public void RotateLocal(Quaternion direction)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, _fixedDeltaTime * _turnSpeed);
        }

        public void ResetToDefault(PlayerCharacterModel data)
        {
            _lastRealPosition = data.Position.Value;
            _realPosition = data.Position.Value;
            _user.Character.ResetToDefaults(data);
        }

        private void UpdateYPosition(float currentYPos)
        {
            if (Math.Abs(currentYPos - _lastYPosition) > 0.1)
            {
                if (_canSendNetworkMovement)
                {
                    _canSendNetworkMovement = false;
                    StartSendingMove();
                }
                _lastYPosition = transform.position.y;
            }
        }

        private void NetworkLerp()
        {
            if (_isLerpingPosition)
            {
                float lerpPercentage = (Time.time - _timeStartedLerping) / _timeToLerp;
                transform.position = Vector3.Lerp(_lastRealPosition, _realPosition, lerpPercentage);
                _user.Character.Position.SetValueAndForceNotify(transform.position);
                _isLerpingPosition = lerpPercentage <= 1;
                _animator.SetFloat("moveSpeed", (_lastRealPosition - _realPosition).magnitude);
            }
            else
            {
                _animator.SetFloat("moveSpeed", 0);
            }

            if (_isLerpingRotation)
            {
                float lerpPercentage = (Time.time - _timeStartedLerping) / _timeToLerp;
                transform.rotation = Quaternion.Lerp(_lastRealRotation, _realRotation, lerpPercentage);
                _isLerpingRotation = lerpPercentage <= 1;
            }

        }

        private void ReadInput()
        {

#if UNITY_EDITOR
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
#else
            var horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            var vertical = CrossPlatformInputManager.GetAxis("Vertical");
#endif
            //horizontal = horizontal > 0 ? 1 : horizontal < 0 ? -1 : 0;
            //vertical = vertical > 0 ? 1 : vertical < 0 ? -1 : 0;

            if (!horizontal.Equals(0) && !vertical.Equals(0))
            {
                horizontal = horizontal * 0.7f;
                vertical = vertical * 0.7f;
            }

            _movementForce = new Vector3(horizontal, 0, vertical);
        }

        /// <summary>
        /// first person movement
        /// </summary>
        private void Update_shooter()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var movement = new Vector3(horizontal, 0, vertical);

            _animator.SetFloat("speed", vertical);

            transform.Rotate(Vector3.up, horizontal * _turnSpeed * Time.fixedDeltaTime);

            if (!vertical.Equals(0))
            {
                _characterController.SimpleMove(transform.forward * _moveSpeed * vertical);
            }
        }

        public void Dispose()
        {
            _movementTask?.Dispose();
            _stumbleTask?.Dispose();
        }

        public void Push(Vector3 direction)
        {
            BlockMovement(true);
            var elapsedTime = 0f;
            _stumbleTask?.Dispose();
            _stumbleTask = Observable.Interval(TimeSpan.FromMilliseconds(10)).Subscribe(frameTime =>
            {
                if (elapsedTime >= _settings.AnimationSetting.StumbleAnimationTime)
                {
                    _stumbleTask?.Dispose();
                    BlockMovement(false);
                    _movementForce = Vector3.zero;
                    return;
                }
                elapsedTime += 10;
                _movementForce = direction * _settings.ArenaSettings.PushForce;
            });
        }

        public void BlockMovement(bool block)
        {
            _blockMovement = block;
            if(block)
                _movementForce = Vector3.zero;
        }
    }
}
