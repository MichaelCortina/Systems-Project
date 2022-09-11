using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _acceleration;
    [SerializeField] private bool _isSpeedCapped;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private MovementMode _movementMode;
    
    private Rigidbody2D _rb;
    private PhotonView _view;

    private void MovePlayer()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (_movementMode == MovementMode.Exponential)
            _rb.AddForce(input * _acceleration * Time.deltaTime);
        else if (_movementMode == MovementMode.Linear)
            _rb.velocity = input * _acceleration * Time.deltaTime;
        
        if (_isSpeedCapped && _rb.velocity.magnitude > _maxSpeed) 
            _rb.velocity = _rb.velocity.normalized * _maxSpeed;
    }
    
    private void RotateToVelocity()
    {
        Vector2 dir = _rb.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    private void Update()
    {
        if (!_view.IsMine) return;
        MovePlayer();
        RotateToVelocity();
    }
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _view = GetComponent<PhotonView>();
    }
}

internal enum MovementMode
{
    Linear,
    Exponential
}
