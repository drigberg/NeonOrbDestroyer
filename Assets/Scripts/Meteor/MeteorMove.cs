using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMove : MonoBehaviour
{
    public Transform groundCheck;
    public Transform ceilingCheck;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public Transform playerCheck;

    public float collisionDistance = 0.1f;
    public LayerMask groundMask;
    public LayerMask playerMask;

    public float maxSpeed = 20f;
    public float gravity = -40f;
    public float bounceHeight = 4f;

    private Vector3 velocity;

    void Start() {
        velocity.x = Random.Range(maxSpeed * -0.85f, maxSpeed * 0.85f);
        velocity.y = Mathf.Sqrt(velocity.x * velocity.x + maxSpeed * maxSpeed) * -1f;
    }

    void Update()
    {
        // check for collisions
        bool isGrounded = Physics.CheckSphere(groundCheck.position, collisionDistance, groundMask);
        bool onWallLeft = Physics.CheckSphere(wallCheckLeft.position, collisionDistance, groundMask);
        bool onWallRight = Physics.CheckSphere(wallCheckRight.position, collisionDistance, groundMask);
        bool onCeiling = Physics.CheckSphere(ceilingCheck.position, collisionDistance, groundMask);
        bool hittingPlayer = Physics.CheckSphere(playerCheck.position, 0.6f + collisionDistance, playerMask);

        // explode if hitting player or ground
        if (hittingPlayer) {
            Explode();
        } else if (isGrounded) {
            Explode();
        }

        // bounce off walls
        bool hittingWall = (onWallLeft && velocity.x < 0) || (onWallRight && velocity.x > 0);
        if (hittingWall) {
            velocity.x *= -1f;
        }

        transform.Translate(velocity * Time.deltaTime);
    }

    void Explode() {
        Destroy(gameObject);
    }
}
