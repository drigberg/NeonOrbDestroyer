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

    [Header ("Wall Gates")]
    public Transform leftWall;
    public Transform rightWall;
    public float wallMoveSpeed = 5f;
    public int countdownSeconds = 3;
    public float leftWallLoweredY;
    public float rightWallRaisedY;
    private float leftWallRaisedY;

    private bool loweringLeftWall = false;
    private bool raisingRightWall = false;
    private bool raisingLeftWall = false;

    // Start is called before the first frame update
    void Start() {
        leftWallRaisedY = leftWall.position.y;
        ui.HideCountdown();
        hearts.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (loweringLeftWall) {
            leftWall.Translate(Vector3.up * -1f * wallMoveSpeed * Time.deltaTime);
            if (leftWall.position.y <= leftWallLoweredY) {
                leftWall.position = new Vector3(leftWall.position.x, leftWallLoweredY, leftWall.position.z);
                loweringLeftWall = false;
                StartCoroutine("CountdownToStart");
            }
        } else if (raisingLeftWall) {
            leftWall.Translate(Vector3.up * wallMoveSpeed * Time.deltaTime);
            if (leftWall.position.y >= leftWallRaisedY) {
                leftWall.position = new Vector3(leftWall.position.x, leftWallRaisedY, leftWall.position.z);
                raisingLeftWall = false;
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
        player.activeArena = this;
        player.cameraLock = PlayerController.CameraLockMode.LOCKING_TO_ARENA;
        hearts.gameObject.SetActive(true);
        BeginLoweringLeftWall();
    }

    public void End() {
        player.cameraLock = PlayerController.CameraLockMode.LOCKING_TO_PLAYER;
        hearts.gameObject.SetActive(false);
        BeginRaisingWalls();
        objectGenerator.Disable();
        backgroundMusic.Pause();
        // TODO: play victory music after delay
    }

    void BeginLoweringLeftWall() {
        loweringLeftWall = true;
    }

    void BeginRaisingWalls() {
        raisingLeftWall = true;
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
