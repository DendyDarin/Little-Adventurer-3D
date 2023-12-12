using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    public float moveSpeed = 5f;
    private Vector3 _movementVelocity;
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    public float gravity = -9.8f;
    private Animator _animator;

    // enemy parameters
    public bool isPlayer = true;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform targetPlayer;

    // state machine
    public enum CharacterState
    {
        normal,
        attacking
    }

    public CharacterState currentState;

    private void Awake() 
    {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // for enemy
        if (!isPlayer)
        {
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            targetPlayer = GameObject.FindWithTag("Player").transform;
            _navMeshAgent.speed = moveSpeed;
        }
        else
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }

    // calculate player movement
    private void CalculatePlayerMovement()
    {
        if (_playerInput.mouseButtonDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.attacking);
            return;
        }

        _movementVelocity.Set(_playerInput.horizontalInput, 0f, _playerInput.verticalInput);
        _movementVelocity.Normalize();

        // match movement to camera rotation
        _movementVelocity = Quaternion.Euler(0, -45, 0) * _movementVelocity;

        // apply animator
        _animator.SetFloat("Speed", _movementVelocity.magnitude);

        // to constant smooth movement across framerate
        _movementVelocity *= moveSpeed * Time.deltaTime;

        // update player rotation while moving
        if (_movementVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        }

        // apply fall animation
        _animator.SetBool("Airborne", !_cc.isGrounded);
    }

    // calculate enemy movement
    private void CalculateEnemyMovement()
    {
        if (Vector3.Distance(targetPlayer.position, transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(targetPlayer.position);
            _animator.SetFloat("Speed", 0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat("Speed", 0f);
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case CharacterState.normal:
                if (isPlayer)
                {
                    CalculatePlayerMovement();
                }
                else
                {
                    CalculateEnemyMovement();
                }
                break;
            case CharacterState.attacking:
                break;
        }
        

        if (isPlayer)
        {
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

    private void SwitchStateTo(CharacterState newState)
    {
        // clear cache
        _playerInput.mouseButtonDown = false;

        // exiting state
        switch (currentState)
        {
            case CharacterState.normal:
                break;
            case CharacterState.attacking:
                break;
        }

        // entering new state
        switch (newState)
        {
            case CharacterState.normal:
                break;
            case CharacterState.attacking:
                _animator.SetTrigger("Attack");
                break;
        }

        currentState = newState;

        Debug.Log("Switched to " + currentState);
    }

    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.normal);
    }
}
