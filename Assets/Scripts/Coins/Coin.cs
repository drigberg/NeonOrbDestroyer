using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 100;
    public Transform groundCheck;
    public MeteorTrail trailPrefab;
    public Explosion explosionPrefab;
    public MeshRenderer mesh;

    public Material defaultMaterial;
    public Material blinkingMaterial;


    public float collisionDistance = 0.1f;
    public LayerMask groundMask;

    public float fallingSpeed = 7.5f;
    public float waitTimeSeconds = 10f;
    public float blinkingSeconds = 3f;
    public int numBlinks = 3;

    private MeteorTrail trail;
    private Vector3 velocity;
    private enum Mode {FALLING, WAITING};
    private Mode mode;

    private bool isGrounded;
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
        trail = Instantiate(trailPrefab, transform.position, rotation);
        smoking = true;
    }

    void Update()
    {
        // check for collisions
        isGrounded = Physics.CheckSphere(groundCheck.position, collisionDistance, groundMask);
        if (mode == Mode.FALLING) {
            Fall();
        }
    }

    void Fall() {
        trail.transform.position = transform.position;
        if (isGrounded) {
            mode = Mode.WAITING;
            velocity.y = 0f;
            trail.StopSmoking();
            smoking = false;
            StartCoroutine("TriggerWaitTimer");
        } else {
            transform.Translate(velocity * Time.deltaTime);
        }
    }

    private IEnumerator TriggerWaitTimer() {
        // wait for a bit
        yield return new WaitForSeconds(waitTimeSeconds - blinkingSeconds);
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
        DestroySelf();
    }

    void DestroySelf() {
        if (smoking) {
            trail.StopSmoking();
        }
        Destroy(gameObject);
    }

    void ExplodeListener(SendExplodeArgs sendExplodeArgs) {
        sendExplodeArgs.points = points;
        Explode();
    }

    void Explode() {
        Instantiate(explosionPrefab, transform.position, new Quaternion());
        DestroySelf();
    }
}
