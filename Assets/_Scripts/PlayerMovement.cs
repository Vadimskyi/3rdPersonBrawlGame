using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 250;
    [SerializeField]
    private float _turnSpeed = 5;

    private CharacterController _characterController;
    private Rigidbody _rigidbody;
    private Animator _animator;

    private Vector3 _movementForce;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 3rd person movement
    /// </summary>
    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        ApplyInput();
    }

    private void ApplyInput()
    {
        //_rigidbody.AddForce(_movementForce * _moveSpeed);
        _characterController.SimpleMove(_movementForce * Time.deltaTime * _moveSpeed);
        _animator.SetFloat("moveSpeed", _movementForce.magnitude);
        if (_movementForce.magnitude > 0)
        {
            Quaternion newDirection = Quaternion.LookRotation(_movementForce);
            transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * _turnSpeed);
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
