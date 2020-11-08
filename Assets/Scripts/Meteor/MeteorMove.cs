using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMove : MonoBehaviour
{
    public Transform groundCheck;
    public Transform ceilingCheck;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public MeteorTrail meteorTrailPrefab;
    public Explosion explosionPrefab;
    private MeteorTrail meteorTrail;

    public float collisionDistance = 0.1f;
    public LayerMask groundMask;

    public float maxSpeed = 20f;

    private Vector3 velocity;
    private enum Mode {FALLING, RUNNING};
    private Mode mode;

    private bool isGrounded;
    private bool onWallLeft;
    private bool onWallRight;
    private bool smoking;

    void Start() {
        StartFalling();
        CreateSmokeTrail();
    }

    void StartFalling() {
        mode = Mode.FALLING;
        velocity.x = Random.Range(maxSpeed * -0.85f, maxSpeed * 0.85f);
        velocity.y = Mathf.Sqrt(velocity.x * velocity.x + maxSpeed * maxSpeed) * -1f;
    }

    void CreateSmokeTrail() {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, velocity * -1f);
        meteorTrail = Instantiate(meteorTrailPrefab, transform.position, rotation);
        smoking = true;
    }

    void Update()
    {
        // check for collisions
        isGrounded = Physics.CheckSphere(groundCheck.position, collisionDistance, groundMask);
        onWallLeft = Physics.CheckSphere(wallCheckLeft.position, collisionDistance, groundMask);
        onWallRight = Physics.CheckSphere(wallCheckRight.position, collisionDistance, groundMask);

        if (mode == Mode.FALLING) {
            Fall();
        } else {
            Run();
        }

    }

    float GetXDirection() {
        return velocity.x > 0f ? 1f : -1f;
    }

    void BounceOffWalls() {
        bool hittingWall = (onWallLeft && velocity.x < 0) || (onWallRight && velocity.x > 0);
        if (hittingWall) {
            velocity.x *= -1f;
            if (smoking) {
                // kill old trail before creating new one
                meteorTrail.StopSmoking();
                CreateSmokeTrail();
            }
        }

    }
    void Fall() {
        BounceOffWalls();
        meteorTrail.transform.position = transform.position;

        if (isGrounded) {
            mode = Mode.RUNNING;
            velocity.x = maxSpeed * GetXDirection();
            velocity.y = 0f;
            meteorTrail.StopSmoking();
            smoking = false;
        }

        transform.Translate(velocity * Time.deltaTime);
    }

    void Run() {
        BounceOffWalls();
        if (isGrounded) {
            velocity.y = 0f;
        } else {
            velocity.x = Mathf.Sqrt(maxSpeed * maxSpeed / 2f) * GetXDirection();
            velocity.y = Mathf.Sqrt(maxSpeed * maxSpeed / 2f) * -1f;
        }
        transform.Translate(velocity * Time.deltaTime);
    }

    void Explode() {
        if (smoking) {
            meteorTrail.StopSmoking();
        }
        Instantiate(explosionPrefab, transform.position, new Quaternion());
        Destroy(gameObject);
    }
}
