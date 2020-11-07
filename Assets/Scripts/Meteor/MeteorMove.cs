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
    public GameObject meteorTrailPrefab;
    private GameObject meteorTrail;

    public float collisionDistance = 0.1f;
    public LayerMask groundMask;
    public LayerMask playerMask;

    public float maxSpeed = 20f;
    public float gravity = -40f;
    public float bounceHeight = 4f;

    private Vector3 velocity;
    private enum Mode {FALLING, RUNNING};
    private Mode mode;

    private bool isGrounded;
    private bool onWallLeft;
    private bool onWallRight;
    private bool onCeiling;
    private bool hittingPlayer;
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
        meteorTrail = Instantiate(meteorTrailPrefab, transform.position, new Quaternion());
        RotateSmokeTrail();
        smoking = true;
    }

    void RotateSmokeTrail() {
        meteorTrail.transform.rotation = Quaternion.FromToRotation(Vector3.up, velocity * -1f);
    }

    void Update()
    {
        // check for collisions
        isGrounded = Physics.CheckSphere(groundCheck.position, collisionDistance, groundMask);
        onWallLeft = Physics.CheckSphere(wallCheckLeft.position, collisionDistance, groundMask);
        onWallRight = Physics.CheckSphere(wallCheckRight.position, collisionDistance, groundMask);
        onCeiling = Physics.CheckSphere(ceilingCheck.position, collisionDistance, groundMask);
        hittingPlayer = Physics.CheckSphere(playerCheck.position, 0.6f + collisionDistance, playerMask);

        // explode if hitting player or ground
        if (hittingPlayer) {
            Explode();
        }

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
        }
        if (smoking) {
            RotateSmokeTrail();
        }
    }
    void Fall() {
        BounceOffWalls();
        meteorTrail.transform.position = transform.position;

        if (isGrounded) {
            mode = Mode.RUNNING;
            velocity.x = maxSpeed * GetXDirection();
            velocity.y = 0f;
            StopSmoking();
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
        StopSmoking();
        Destroy(gameObject);
    }

    void StopSmoking() {
        if (smoking) {
            Destroy(meteorTrail);
            smoking = false;
        }
    }
}
