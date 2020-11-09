using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMove : MonoBehaviour
{
    public int points = 25;
    public Transform groundCheck;
    public Transform ceilingCheck;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public MeteorTrail meteorTrailPrefab;
    public Explosion explosionPrefab;
    private MeteorTrail meteorTrail;
    public MeshRenderer mesh;

    public Material defaultMaterial;
    public Material blinkingMaterial;

    public float collisionDistance = 0.1f;
    public LayerMask groundMask;

    public float fallingSpeed = 7.5f;
    public float maxSpeed = 15f;

    private Vector3 velocity;
    private enum Mode {FALLING, RUNNING};
    private Mode mode;

    public float lifetimeSeconds = 10f;
    public float blinkingSeconds = 3f;
    public int numBlinks = 3;

    private bool isGrounded;
    private bool onWallLeft;
    private bool onWallRight;
    private bool smoking;

    void Start() {
        mesh.material = defaultMaterial;
        StartFalling();
        CreateSmokeTrail();
    }

    void StartFalling() {
        mode = Mode.FALLING;
        velocity.y = fallingSpeed * -1f;
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
            StartCoroutine("TriggerSelfDestructTimer");
            mode = Mode.RUNNING;
            velocity.x = maxSpeed * GetXDirection();
            velocity.y = 0f;
            meteorTrail.StopSmoking();
            smoking = false;
        }

        transform.Translate(velocity * Time.deltaTime);
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

    void ExplodeListener(SendExplodeArgs sendExplodeArgs) {
        if (!sendExplodeArgs.invincible) {
            bool explode = false;
            if (sendExplodeArgs.attacking) {
                sendExplodeArgs.points = points;
                explode = true;
            } else {
                sendExplodeArgs.dealDamage = true;
            }
            DestroySelf(explode);
        }
    }

    void DestroySelf(bool explode) {
        if (smoking) {
            meteorTrail.StopSmoking();
        }
        if (explode) {
            Explode();
        }
        Destroy(gameObject);
    }

    void Explode() {
        Instantiate(explosionPrefab, transform.position, new Quaternion());
    }
}
