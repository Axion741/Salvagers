﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Camera _cam;

    private float moveSpeed = 5f;
    private float _xMovement;
    private float _yMovement;
    private float _mouseZ = 5.23f;
    private float _turnAngle;
    private Vector3 _mousePosition;
    private Vector3 _currentPosition;


    private void Awake()
    {
       _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        _xMovement = Input.GetAxisRaw("Horizontal");
        _yMovement = Input.GetAxisRaw("Vertical");

        RotateToMouse();
    }

    private void FixedUpdate()
    {
        var normalizedVelocity = new Vector2(_xMovement, _yMovement).normalized; //.normalized stops diagonal movement being faster
        _rb.velocity = normalizedVelocity * moveSpeed; 
    }

    private void RotateToMouse()
    {
        _currentPosition = _cam.WorldToScreenPoint(transform.position);

        _mousePosition = Input.mousePosition;
        _mousePosition.z = _mouseZ;
        _mousePosition.x = _mousePosition.x - _currentPosition.x;
        _mousePosition.y = _mousePosition.y - _currentPosition.y;

        _turnAngle = Mathf.Atan2(_mousePosition.y, _mousePosition.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _turnAngle));
    }

}