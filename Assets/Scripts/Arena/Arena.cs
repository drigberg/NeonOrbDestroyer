using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [Header ("Controllers")]
    public PlayerController player;
    public Hearts hearts;
    public ObjectGenerator objectGenerator;
    public GlowGauge glowGauge;
    public UI ui;

    [Header ("Audio")]
    public AudioPlayer backgroundMusic;
    public AudioPlayer gameOverMusic;

    [Header ("Camera")]
    public float cameraLockX = 0f;
    public float cameraLockSpeed = 1f;
    public Transform mainCamera;
    private Vector3 cameraLockPosition;

    [Header ("Wall Gates")]
    public Transform leftWall;
    public Transform rightWall;
    public float wallMoveSpeed = 5f;
    public int countdownSeconds = 3;
    public float leftWallLoweredY;
    public float rightWallRaisedY;

    private enum LockMode {LOCKED_TO_PLAYER, LOCKING_TO_PLAYER, LOCKED_TO_ARENA, LOCKING_TO_ARENA};
    private LockMode cameraLock = LockMode.LOCKED_TO_PLAYER;
    private bool loweringLeftWall = false;
    private bool raisingRightWall = false;

    // Start is called before the first frame update
    void Start() {
        cameraLockPosition = new Vector3(cameraLockX, mainCamera.position.y, mainCamera.position.z);
        ui.HideCountdown();
        hearts.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (cameraLock == LockMode.LOCKED_TO_PLAYER) {
            mainCamera.position = new Vector3(player.transform.position.x, mainCamera.position.y, mainCamera.position.z);
        } else if (cameraLock == LockMode.LOCKING_TO_ARENA) {
            mainCamera.position = Vector3.MoveTowards(mainCamera.position, cameraLockPosition, cameraLockSpeed * Time.deltaTime);
            if (Vector3.Distance(mainCamera.position, cameraLockPosition) < 0.1f) {
                mainCamera.position = cameraLockPosition;
                cameraLock = LockMode.LOCKED_TO_ARENA;
            }
        } else if (cameraLock == LockMode.LOCKING_TO_PLAYER) {
            Vector3 playerLockPosition = new Vector3(player.transform.position.x, mainCamera.position.y, mainCamera.position.z);
            mainCamera.position = Vector3.MoveTowards(mainCamera.position, playerLockPosition, cameraLockSpeed * Time.deltaTime);
            if (Vector3.Distance(mainCamera.position, playerLockPosition) < 0.1f) {
                mainCamera.position = playerLockPosition;
                cameraLock = LockMode.LOCKED_TO_PLAYER;
            }
        }

        if (loweringLeftWall) {
            leftWall.Translate(Vector3.up * -1f * wallMoveSpeed * Time.deltaTime);
            if (leftWall.position.y <= leftWallLoweredY) {
                leftWall.position = new Vector3(leftWall.position.x, leftWallLoweredY, leftWall.position.z);
                loweringLeftWall = false;
                StartCoroutine("CountdownToStart");
            }
        }
        if (raisingRightWall) {
            rightWall.Translate(Vector3.up * wallMoveSpeed * Time.deltaTime);
            if (rightWall.position.y >= rightWallRaisedY) {
                rightWall.position = new Vector3(rightWall.position.x, rightWallRaisedY, rightWall.position.z);
                raisingRightWall = false;
            }
        }
    }

    public void Begin() {
        cameraLock = LockMode.LOCKING_TO_ARENA;
        hearts.gameObject.SetActive(true);
        BeginLoweringLeftWall();
    }

    public void End() {
        cameraLock = LockMode.LOCKING_TO_PLAYER;
        hearts.gameObject.SetActive(false);
        BeginRaisingRightWall();
        objectGenerator.Disable();
        backgroundMusic.Pause();
        // TODO: play victory music after delay
    }

    void BeginLoweringLeftWall() {
        loweringLeftWall = true;
    }

    void BeginRaisingRightWall() {
        raisingRightWall = true;
    }

    public void GameOver() {
        backgroundMusic.Pause();
        gameOverMusic.PlayAfterDelay();
        objectGenerator.Disable();
    }

    private IEnumerator CountdownToStart() {
        objectGenerator.Reset();
        int steps = 0;
        while (steps < countdownSeconds) {
            ui.SetCountdown(countdownSeconds - steps);
            yield return new WaitForSeconds(1);
            steps += 1;
        }
        ui.CountDownZero();
        yield return new WaitForSeconds(1);
        ui.HideCountdown();
        objectGenerator.Enable();
        backgroundMusic.Play();
    }
}
