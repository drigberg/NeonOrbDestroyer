using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header ("UI")]
    public MenuController menuController;

    [Header ("Controllers")]
    public Arena activeArena;
    public CharacterController controller;

    [Header ("Animation")]
    public Animator animator;
    public Transform bodyTransform;
    public Weapon weaponPrefab;
    public Transform rightElbow;
    public Transform rightHand;
    private Weapon weapon;

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
    public float wallJumpGracePeriodDuration = 0.06f;

    [Header ("Mesh")]
    public SkinnedMeshRenderer[] meshes;
    public Material defaultMaterial;
    public Material invincibilityMaterial;

    [Header ("Movement")]
    public float speed = 20f;
    public float gravity = -120f;
    public float maxWallSlidingSpeed = -10f;
    public float jumpHeight = 6f;

    [Header ("Timers")]
    public float attackDuration = 0.25f;
    public float attackDelayDuration = 0.1f;
    public float invincibilityDuration = 1.0f;

    [Header ("Audio")]
    public AudioClip jumpSound;
    public AudioClip attackSound;
    private AudioSource audioSource;
    public AudioPlayer backgroundMusic;
    public AudioPlayer gameOverMusic;

    // State
    private bool facingRight;
    private bool hasWallJumpedOnThisWall;
    private bool inWallJumpGracePeriod;
    private bool onWallOrInGracePeriod;
    private bool physicallyOnWall;
    private bool canWallJump;
    private bool attacking;
    private bool attackingDelayed;
    private bool invincible;
    private bool isGrounded;
    private bool onWallLeft;
    private bool onWallRight;
    private bool onCeiling;
    private Vector3 velocity;

    void SetMesh(Material material) {
        for (int i = 0; i < meshes.Length; i++) {
            meshes[i].materials[0] = material;
            meshes[i].material = material;
        }
    }

    void Start() {
        weapon = Instantiate(weaponPrefab, rightHand.position, rightHand.rotation);
        weapon.transform.parent = rightHand.transform;
        audioSource = GetComponent<AudioSource>();
        SetMesh(defaultMaterial);
    }

    void Update() {
        CheckForCollisions();
        HandleNonMovementInput();
        Move();
        HandleWeapon();
        ListenForPause();
    }

    void ListenForPause() {
        if (Input.GetKeyDown(KeyCode.P)) {
            menuController.ShowScreenByIndex(1);
            Time.timeScale = 0f;
        }
    }

    void HandleWeapon() {
        // Keeps the weapon in the player's right hand, aligned outwards with the forearm
        // Only shows the weapon if attacking
        weapon.transform.position = rightHand.position;
        weapon.transform.rotation = Quaternion.LookRotation(rightHand.position - rightElbow.position, Vector3.forward);
        weapon.gameObject.SetActive(attacking);
    }

    void CheckForCollisions() {
        // check for platform collisions
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, platformMask);
        onWallLeft = Physics.CheckSphere(wallCheckLeft.position, groundDistance, platformMask);
        onWallRight = Physics.CheckSphere(wallCheckRight.position, groundDistance, platformMask);
        onCeiling = Physics.CheckSphere(ceilingCheck.position, groundDistance, platformMask);

        bool lastFramePhysicallyOnWall = physicallyOnWall;
        physicallyOnWall = onWallLeft || onWallRight;
        onWallOrInGracePeriod = physicallyOnWall || inWallJumpGracePeriod;
        if (lastFramePhysicallyOnWall && !physicallyOnWall) {
            StartCoroutine("WallJumpGracePeriod");
        }

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("grippingWallLeft", onWallLeft);
        animator.SetBool("grippingWallRight", onWallRight);


        // check for enemy collisions
        float enemyCheckRadius = enemyCheckDistance;
        if (attacking) {
            enemyCheckRadius += attackReach;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyCheckRadius, enemyMask);
        bool damageDealt = false;
        float collectedGlow = 0;
        foreach (var hitCollider in hitColliders) {
            // tell the other object that we hit it and pass a mutable object to get data back
            SendExplodeArgs sendExplodeArgs = new SendExplodeArgs();
            sendExplodeArgs.invincible = invincible;
            sendExplodeArgs.attacking = attacking;
            hitCollider.SendMessage("ExplodeListener", sendExplodeArgs);

            // listening object determines whether damage is dealt, and whether glow is received
            if (sendExplodeArgs.dealDamage) {
                damageDealt = true;
            }
            // glow defaults to 0, so we can always add them
            // NOTE: if an enemy hits us in the same frame we're getting a coin, we just accept the coin glow
            collectedGlow += sendExplodeArgs.glow;
        }

        activeArena.glowGauge.AddGlow(collectedGlow);
        // we only take damage once per frame, even if we're hit by multiple enemies
        if (damageDealt) {
            TakeDamage();
        }
    }

    public void TakeDamage() {
        Explode();
        int heartsRemaining = activeArena.hearts.SubtractOne();
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
        backgroundMusic.Pause();
        gameOverMusic.PlayAfterDelay();
        activeArena.objectGenerator.Disable();
        menuController.ShowScreenByIndex(0);
        Destroy(gameObject);
    }

    void HandleNonMovementInput() {
        if (Input.GetKeyDown(KeyCode.M) && !attacking && !invincible && !attackingDelayed) {
            StartCoroutine("Attack");
        }
    }

    void Jump() {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // only allow one wall jump between touching the ground
        if (onWallOrInGracePeriod) {
            hasWallJumpedOnThisWall = true;
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

        bool canJump = isGrounded || (onWallOrInGracePeriod && !hasWallJumpedOnThisWall);
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

    private IEnumerator WallJumpGracePeriod() {
        inWallJumpGracePeriod = true;
        yield return new WaitForSeconds(wallJumpGracePeriodDuration);
        inWallJumpGracePeriod = false;
        hasWallJumpedOnThisWall = false;
    }

    private IEnumerator TriggerInvincibility() {
        invincible = true;
        SetMesh(invincibilityMaterial);
        yield return new WaitForSeconds(invincibilityDuration);
        SetMesh(defaultMaterial);
        invincible = false;
    }

    private IEnumerator Attack() {
        attacking = true;
        PlaySound(attackSound);
        animator.SetBool("attacking", true);
        yield return new WaitForSeconds(attackDuration);
        attacking = false;
        animator.SetBool("attacking", false);
        StartCoroutine("TriggerAttackDelay");
    }

    private IEnumerator TriggerAttackDelay() {
        attackingDelayed = true;
        yield return new WaitForSeconds(attackDelayDuration);
        attackingDelayed = false;
    }

    void PlaySound(AudioClip sound) {
        if (!GameSettings.muted) {
            audioSource.PlayOneShot(sound);
        }
    }
}
