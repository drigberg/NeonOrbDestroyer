using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Hearts hearts;

    public CharacterController controller;
    public Transform groundCheck;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public Transform ceilingCheck;
    public float groundDistance = 0.2f;
    public LayerMask platformMask;
    public LayerMask enemyMask;

    public MeshRenderer mesh;
    public Material defaultMaterial;
    public Material attackingMaterial;
    public Material invincibilityMaterial1;
    public Material invincibilityMaterial2;

    public float speed = 20f;
    public float gravity = -120f;
    public float maxWallSlidingSpeed = -10f;
    public float jumpHeight = 6f;

    public float enemyCheckDistance = 0.5f;
    public float attackReach = 0.25f;

    private bool attacking;
    private bool attackingDelayed;
    private bool invincible;

    public float attackDuration = 0.25f;
    public float attackDelayDuration = 0.1f;
    public float invincibilityStepDuration = 0.3f;
    public int invincibilitySteps = 4;

    public Explosion explosionPrefab;

    private bool isGrounded;
    private bool onWallLeft;
    private bool onWallRight;
    private bool onCeiling;

    private Vector3 velocity;

    void Start() {
        mesh.material = defaultMaterial;
    }

    void Update() {
        CheckForCollisions();
        HandleNonMovementInput();
        Move();
    }

    void CheckForCollisions() {
        // check for platform collisions
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, platformMask);
        onWallLeft = Physics.CheckSphere(wallCheckLeft.position, groundDistance, platformMask);
        onWallRight = Physics.CheckSphere(wallCheckRight.position, groundDistance, platformMask);
        onCeiling = Physics.CheckSphere(ceilingCheck.position, groundDistance, platformMask);

        // check for enemy collisions
        if (!invincible) {
            float enemyCheckRadius = enemyCheckDistance;
            if (attacking) {
                enemyCheckRadius += attackReach;
            }
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyCheckDistance, enemyMask);
            foreach (var hitCollider in hitColliders) {
                hitCollider.SendMessage("Explode");
            }
            if (hitColliders.Length > 0 && !attacking) {
                TakeDamage();
            }
        }
    }

    public void TakeDamage() {
        Explode();
        int heartsRemaining = hearts.SubtractOne();
        if (heartsRemaining > 0) {
            StartCoroutine("TriggerInvincibility");
        } else {
            GameOver();
        }
    }

    void Explode() {
        Instantiate(explosionPrefab, transform.position, new Quaternion());
    }

    void GameOver() {
        Destroy(gameObject);
    }

    void HandleNonMovementInput() {
        if (Input.GetKeyDown(KeyCode.M) && !attacking && !invincible && !attackingDelayed) {
            StartCoroutine("Attack");
        }
    }

    void Move() {
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
        if (transform.position.z != 0f) {
            Vector3 movementOffSet = new Vector3(0, 0, (0 - transform.position.z) * 0.05f);
            controller.Move(movementOffSet);
        }
    }

    private IEnumerator TriggerInvincibility() {
        invincible = true;
        int steps = 0;
        while (steps < invincibilitySteps) {
            mesh.material = invincibilityMaterial1;
            yield return new WaitForSeconds(invincibilityStepDuration);
            mesh.material = invincibilityMaterial2;
            yield return new WaitForSeconds(invincibilityStepDuration);
            steps += 1;
        }
        mesh.material = defaultMaterial;
        invincible = false;
    }

    private IEnumerator Attack() {
        attacking = true;
        mesh.material = attackingMaterial;
        yield return new WaitForSeconds(attackDuration);
        mesh.material = defaultMaterial;
        attacking = false;
        StartCoroutine("TriggerAttackDelay");
    }

    private IEnumerator TriggerAttackDelay() {
        attackingDelayed = true;
        yield return new WaitForSeconds(attackDelayDuration);
        attackingDelayed = false;
    }
}
