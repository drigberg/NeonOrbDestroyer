using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public Transform ceilingCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    public float speed = 20f;
    public float gravity = -120f;
    public float maxWallSlidingSpeed = -10f;
    public float jumpHeight = 6f;

    private Vector3 velocity;

    void Update()
    {
        // check for collisions
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        bool onWallLeft = Physics.CheckSphere(wallCheckLeft.position, groundDistance, groundMask);
        bool onWallRight = Physics.CheckSphere(wallCheckRight.position, groundDistance, groundMask);
        bool onCeiling = Physics.CheckSphere(ceilingCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f) {
            // stop moving downwards on floors
            velocity.y = -2f;
        } else if (onCeiling && velocity.y > 0f) {
            // stop moving upwards on ceilings
            velocity.y = 0f;
        }

        // jump from ground or wall
        bool canJump = isGrounded || onWallLeft || onWallRight;
        if (Input.GetButtonDown("Jump") && canJump) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // move left and right
        float x = Input.GetAxisRaw("Horizontal");
        if (x > 0) {
            velocity.x = speed;
        } else if (x < 0) {
            velocity.x = speed * -1f;
        } else {
            velocity.x = 0;
        }

        // slide downwards more slowly when gripping the wall
        bool grippingWall = (onWallLeft && x < 0) || (onWallRight && x > 0);
        velocity.y += gravity * Time.deltaTime;
        if (grippingWall && velocity.y < maxWallSlidingSpeed) {
            velocity.y = maxWallSlidingSpeed;
        }
        controller.Move(velocity * Time.deltaTime);
    }
}
