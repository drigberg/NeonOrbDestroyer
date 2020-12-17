using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenMovement : MonoBehaviour
{
    public int glow = 25;

    [Header ("Animation")]
    public Animator animator;
    public SkinnedMeshRenderer mesh;
    public Transform bodyTransform;

    [Header ("Prefabs")]
    public Explosion explosionPrefab;

    [Header ("Materials")]
    public Material defaultMaterial;
    public Material blinkingMaterial;

    [Header ("Collisions")]
    public float collisionDistance = 0.1f;
    public LayerMask platformMask;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public Transform ceilingCheck;
    public Transform groundCheck;

    [Header ("Movement")]
    public float fallingSpeed = 5f;
    public float maxSpeed = 10f;
    private Vector3 velocity;
    private Vector3 externalMovement;
    private enum Mode {FALLING, RUNNING};
    private Mode mode;

    [Header ("Lifetime")]
    public float lifetimeSeconds = 10f;
    public float blinkingSeconds = 3f;
    public int numBlinks = 3;

    // State
    private bool isGrounded;
    private bool onWallLeft;
    private bool onWallRight;

    void Start() {
        mesh.material = defaultMaterial;
        StartFalling();
    }

    void StartFalling() {
        mode = Mode.FALLING;
        velocity.y = fallingSpeed * -1f;
    }

    void Update()
    {
        // check for collisions
        isGrounded = Physics.CheckSphere(groundCheck.position, collisionDistance, platformMask);
        onWallLeft = Physics.CheckSphere(wallCheckLeft.position, collisionDistance, platformMask);
        onWallRight = Physics.CheckSphere(wallCheckRight.position, collisionDistance, platformMask);

        animator.SetBool("isGrounded", isGrounded);

        if (mode == Mode.FALLING) {
            Fall();
        } else {
            Run();
        }
        Rotate();
        GetPlatformMovement();
    }

    void FixedUpdate() {
        transform.Translate(velocity * Time.deltaTime + externalMovement);
    }

    float GetXDirection() {
        return velocity.x > 0f ? 1f : -1f;
    }

    void BounceOffWalls() {
        bool hittingWall = (onWallLeft && velocity.x < 0) || (onWallRight && velocity.x > 0);
        if (hittingWall) {
            velocity.x *= -1f;
        }
    }

    void Fall() {
        BounceOffWalls();

        if (isGrounded) {
            StartCoroutine("TriggerSelfDestructTimer");
            mode = Mode.RUNNING;
            velocity.x = maxSpeed * GetXDirection();
            velocity.y = 0f;
        }
    }

    void Run() {
        BounceOffWalls();
        if (isGrounded) {
            velocity.y = 0f;
        } else {
            velocity.x = Mathf.Sqrt(maxSpeed * maxSpeed / 2f) * GetXDirection();
            velocity.y = Mathf.Sqrt(maxSpeed * maxSpeed / 2f) * -1f;
        }
    }

    void GetPlatformMovement() {
        RaycastHit hit;
        // the raven's position is too low, so we have to start casting from mid-body
        if (isGrounded && Physics.Raycast(transform.position + Vector3.up * 1.0f, transform.TransformDirection(Vector3.down), out hit, 2.0f, platformMask)) {
            MovingPlatform movingPlatform = hit.transform.GetComponent<MovingPlatform>();
            if (movingPlatform) {
                externalMovement = movingPlatform.velocity;
            }
        } else {
            externalMovement = Vector3.zero;
        }
    }

    private IEnumerator TriggerSelfDestructTimer() {
        // wait for a bit
        yield return new WaitForSeconds(lifetimeSeconds - blinkingSeconds);

        // blink for final seconds before exploding
        int steps = 0;
        float blinkStepDuration = blinkingSeconds / (float)numBlinks;
        while (steps < numBlinks) {
            mesh.material = blinkingMaterial;
            yield return new WaitForSeconds(blinkStepDuration / 2f);
            mesh.material = defaultMaterial;
            yield return new WaitForSeconds(blinkStepDuration / 2f);
            steps += 1;
        }
        DestroySelf(false);
    }

    void Rotate() {
        int targetRotation = 0;
        if (mode == Mode.RUNNING) {
            float direction = GetXDirection();
            if (direction > 0) {
                targetRotation = 270;
            } else {
                targetRotation = 90;
            }
        }
        bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, Quaternion.Euler(0, targetRotation, 0), 0.1f);
    }

    public void ExplodeListener(SendExplodeArgs sendExplodeArgs) {
        if (!sendExplodeArgs.invincible) {
            bool explode = false;
            if (sendExplodeArgs.attacking) {
                sendExplodeArgs.glow = glow;
                explode = true;
            } else {
                sendExplodeArgs.dealDamage = true;
            }
            DestroySelf(explode);
        }
    }

    public void ForceDestroyAndExplode() {
        DestroySelf(true);
    }

    void DestroySelf(bool explode) {
        if (explode) {
            Explode();
        }
        Destroy(gameObject);
    }

    void Explode() {
        Instantiate(explosionPrefab, transform.position, new Quaternion());
    }
}
