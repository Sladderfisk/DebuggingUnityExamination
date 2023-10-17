using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float slowDownSpeed;
    
    [SerializeField] private float rotationSpeed;

    private float currentSpeed;
    private Vector2 moveDirection;

    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection = moveDirection.normalized;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        if (moveDirection == Vector2.zero) SlowDown();
        else SpeedUp();
    }

    private void SpeedUp()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, playerSpeed, accelerationSpeed * Time.fixedDeltaTime);
        playerRb.velocity = transform.up * currentSpeed;
    }

    private void SlowDown()
    {
        currentSpeed = Mathf.MoveTowards(currentSpeed, 0, slowDownSpeed * Time.fixedDeltaTime);
        playerRb.velocity = transform.up * currentSpeed;
    }

    private void RotatePlayer()
    {
        if (moveDirection == Vector2.zero) return;

        var dir = Mathf.Atan2(moveDirection.x, moveDirection.y);
        dir *= Mathf.Rad2Deg * -1;

        var rot = new Quaternion { eulerAngles = new(0, 0, dir) };
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.fixedDeltaTime);
    }
}
