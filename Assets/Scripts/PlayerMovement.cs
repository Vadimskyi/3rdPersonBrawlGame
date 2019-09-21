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
    private Animator _animator;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// 3rd person movement
    /// </summary>
    private void Update()
    {
        //if (!_characterController.isGrounded) return;
#if UNITY_EDITOR
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
#else
        var horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        var vertical = CrossPlatformInputManager.GetAxis("Vertical");
#endif

        var movement = new Vector3(horizontal, 0, vertical);
        _characterController.SimpleMove(movement * Time.deltaTime * _moveSpeed);
        _animator.SetBool("moving", movement.magnitude > 0);
        if (movement.magnitude > 0)
        {
            Quaternion newDirection = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * _turnSpeed);
        }
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
