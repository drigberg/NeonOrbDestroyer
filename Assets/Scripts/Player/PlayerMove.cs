using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    [Header ("UI")]
    public UI ui;
    public MenuButtonController gameOverMenu;
    public Text scoreText;

    [Header ("Animation")]
    public Animator animator;
    public Transform bodyTransform;

    [Header ("Controllers")]
    public CharacterController controller;
    public Hearts hearts;
    public ObjectGenerator objectGenerator;

    [Header ("Prefabs")]
    public Explosion explosionPrefab;

    [Header ("Collisions")]
    public Transform groundCheck;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public Transform ceilingCheck;
    public float groundDistance = 0.2f;
    public LayerMask platformMask;
    public LayerMask enemyMask;
    public float enemyCheckDistance = 0.5f;
    public float attackReach = 0.25f;

    [Header ("Mesh")]
    public SkinnedMeshRenderer[] meshes;
    public Material defaultMaterial;
    public Material attackingMaterial;
    public Material invincibilityMaterial1;
    public Material invincibilityMaterial2;

    [Header ("Movement")]
    public float speed = 20f;
    public float gravity = -120f;
    public float maxWallSlidingSpeed = -10f;
    public float jumpHeight = 6f;


    [Header ("Timers")]
    public float attackDuration = 0.25f;
    public float attackDelayDuration = 0.1f;
    public float invincibilityStepDuration = 0.3f;
    public int invincibilitySteps = 4;

    [Header ("Audio")]
    public AudioClip jumpSound;
    private AudioSource audioSource;

    // State
    private bool facingRight;
    private bool canWallJump;
    private bool attacking;
    private bool attackingDelayed;
    private bool invincible;
    private bool isGrounded;
    private bool onWallLeft;
    private bool onWallRight;
    private bool onCeiling;
    private int points;
    private Vector3 velocity;

    void SetMesh(Material material) {
        for (int i = 0; i < meshes.Length; i++) {
            meshes[i].materials[0] = material;
            meshes[i].material = material;
        }
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        SetMesh(defaultMaterial);
        points = 0;
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

        if (isGrounded) {
            canWallJump = true;
        }
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("grippingWallLeft", onWallLeft);
        animator.SetBool("grippingWallRight", onWallRight);


        // check for enemy collisions
        float enemyCheckRadius = enemyCheckDistance;
        if (attacking) {
            enemyCheckRadius += attackReach;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyCheckDistance, enemyMask);
        bool damageDealt = false;
        foreach (var hitCollider in hitColliders) {
            // tell the other object that we hit it and pass a mutable object to get data back
            SendExplodeArgs sendExplodeArgs = new SendExplodeArgs();
            sendExplodeArgs.invincible = invincible;
            sendExplodeArgs.attacking = attacking;
            hitCollider.SendMessage("ExplodeListener", sendExplodeArgs);

            // listening object determines whether damage is dealt, and whether points are received
            if (sendExplodeArgs.dealDamage) {
                damageDealt = true;
            }
            // points defaults to 0, so we can always add them
            // NOTE: if an enemy hits us in the same frame we're getting a coin, we just accept the coin points
            points += sendExplodeArgs.points;
        }
        ui.SetPoints(points);
        // we only take damage once per frame, even if we're hit by multiple enemies
        if (damageDealt) {
            TakeDamage();
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
        objectGenerator.Disable();
        scoreText.text = "SCORE: " + points;
        gameOverMenu.gameObject.SetActive(true);
    }

    void HandleNonMovementInput() {
        if (Input.GetKeyDown(KeyCode.M) && !attacking && !invincible && !attackingDelayed) {
            StartCoroutine("Attack");
        }
    }

    void Jump() {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // only allow one wall jump between touching the ground
        if (onWallLeft || onWallRight) {
            canWallJump = false;
        }
        PlaySound(jumpSound);
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
        bool canJump = isGrounded || (canWallJump && (onWallLeft || onWallRight));
        if (Input.GetButtonDown("Jump") && canJump) {
            Jump();
        }

        // move left and right
        float x = Input.GetAxisRaw("Horizontal");
        if (x > 0) {
            facingRight = true;
            velocity.x = speed;
            animator.SetBool("movingLeftRight", true);
        } else if (x < 0) {
            facingRight = false;
            velocity.x = speed * -1f;
            animator.SetBool("movingLeftRight", true);
        } else {
            velocity.x = 0;
            animator.SetBool("movingLeftRight", false);
        }

        // rotate based on facing direction
        int targetRotation = facingRight ? 270 : 90;
        if (onWallLeft || onWallRight) {
            targetRotation = 0;
        }
        bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, Quaternion.Euler(0, targetRotation, 0), 0.1f);


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
            SetMesh(invincibilityMaterial1);
            yield return new WaitForSeconds(invincibilityStepDuration);
            SetMesh(invincibilityMaterial2);
            yield return new WaitForSeconds(invincibilityStepDuration);
            steps += 1;
        }
        SetMesh(defaultMaterial);
        invincible = false;
    }

    private IEnumerator Attack() {
        attacking = true;
        SetMesh(attackingMaterial);
        yield return new WaitForSeconds(attackDuration);
        SetMesh(defaultMaterial);
        attacking = false;
        StartCoroutine("TriggerAttackDelay");
    }

    private IEnumerator TriggerAttackDelay() {
        attackingDelayed = true;
        yield return new WaitForSeconds(attackDelayDuration);
        attackingDelayed = false;
    }

    void PlaySound(AudioClip sound) {
        audioSource.PlayOneShot(sound);
    }
}
