using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class OrganismMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private MovementType movementType;

    private Vector2 randomDirection;

    private Transform player;
    private Rigidbody2D myRb;

    private void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        Rotate(Random.Range(0, 360));
    }

    private void FixedUpdate()
    {
        switch (movementType)
        {
            case MovementType.FollowPlayer:
                FollowPlayer();
                break;
            
            case MovementType.MoveRandomDirection:

                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Rotate when collided.
        // DO THIS WEHN YOU SEE IT!!!!
    }

    private void MoveRandomDirection()
    {
        Move(transform.up * movementSpeed);
    }

    private void FollowPlayer()
    {
        var playerDirection = (transform.position - player.position).normalized;
        Move(playerDirection * movementSpeed);
    }
    
    private void Rotate(float newRotation)
    {
        myRb.rotation = newRotation;
    }

    private void Move(Vector2 velocity)
    {
        myRb.velocity = velocity;
    }
    
    private enum MovementType
    {
        FollowPlayer,
        MoveRandomDirection
    }
}
