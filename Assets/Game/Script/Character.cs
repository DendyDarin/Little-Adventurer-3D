using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    public float moveSpeed = 5f;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    public float gravity = -9.8f;

    private void Awake() 
    {
        _cc = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // calculate player movement
    private void CalculatePlayerMovement()
    {
        _movementVelocity.Set(_playerInput.horizontalInput, 0f, _playerInput.verticalInput);
        _movementVelocity.Normalize();

        // match movement to camera rotation
        _movementVelocity = Quaternion.Euler(0, -45, 0) * _movementVelocity;

        // to constant smooth movement across framerate
        _movementVelocity *= moveSpeed * Time.deltaTime;

        // update player rotation while moving
        if (_movementVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }
    }

    private void FixedUpdate() 
    {
        CalculatePlayerMovement();

        // check if the player touch the ground, if not, apply the gravity
        if (_cc.isGrounded == false)
        {
            _verticalVelocity = gravity;
        }
        else
        {
            _verticalVelocity = gravity * 0.3f;
        }

        _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;

        _cc.Move(_movementVelocity);
    }
}
