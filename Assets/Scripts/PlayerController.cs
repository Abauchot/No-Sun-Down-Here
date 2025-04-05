using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerLocomotionInput _playerLocomotionInput;
    
    
    [SerializeField] public float moveSpeed = 5f;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
    }

    private void MovePlayer()
    {
        Vector2 movement = _playerLocomotionInput.MoveInput;
        _rb.linearVelocity = movement * moveSpeed;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
}
