using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Vadimskyi.Game
{
    public class PlayerMovement
    {
        public event Action<Transform, Vector3> OnMove = delegate { };
        public event Action<Quaternion> OnRotate = delegate { };

        [SerializeField]
        private float _moveSpeed = 250;
        [SerializeField]
        private float _turnSpeed = 5;

        private CharacterController _characterController;
        private Rigidbody _rigidbody;
        private Animator _animator;
        private Transform transform;

        private Vector3 _prevMovementForce;
        private Vector3 _movementForce;

        public PlayerMovement(
            Transform target,
            Animator animator,
            Rigidbody rigidbody,
            CharacterController characterController)
        {
            transform = target;
            _characterController = characterController;
            _rigidbody = rigidbody;
            _animator = animator;
        }

        /// <summary>
        /// 3rd person movement
        /// </summary>
        public void CustomUpdate()
        {
            ReadInput();
        }

        public void CustomFixedUpdate()
        {
            if (_movementForce.magnitude.Equals(0) && _movementForce == _prevMovementForce) return;

            OnMove?.Invoke(transform, _movementForce);
            if (_movementForce.magnitude > 0)
            {
                Quaternion newDirection = Quaternion.LookRotation(_movementForce);
                OnRotate?.Invoke(newDirection);
            }
            _prevMovementForce = _movementForce;
        }

        public void Move(Vector3 movementForce)
        {
            _characterController.SimpleMove(movementForce * Time.deltaTime * _moveSpeed);
            _animator.SetFloat("moveSpeed", movementForce.magnitude);
        }

        public void Rotate(Quaternion direction)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, Time.deltaTime * _turnSpeed);
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

            transform.Rotate(Vector3.up, horizontal * _turnSpeed * Time.deltaTime);

            if (!vertical.Equals(0))
            {
                _characterController.SimpleMove(transform.forward * _moveSpeed * vertical);
            }
        }
    }
}
