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
    public Transform mainCamera;

    [Header ("Wall Gates")]
    public Transform leftWall;
    public Transform rightWall;
    public float wallMoveSpeed = 5f;
    public int countdownSeconds = 3;
    public float leftWallLoweredY;
    public float rightWallRaisedY;

    [Header ("State")]
    private bool activated = false;
    private bool loweringLeftWall = false;
    private bool raisingRightWall = false;

    // Start is called before the first frame update
    void Start() {
        ui.HideCountdown();
        hearts.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated) {
            mainCamera.position = new Vector3(player.transform.position.x, mainCamera.position.y, mainCamera.position.z);
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
                rightWall.position = new Vector3(rightWall.position.x, leftWallLoweredY, rightWall.position.z);
                raisingRightWall = false;
            }
        }
    }

    void BeginLoweringLeftWall() {
        loweringLeftWall = true;
    }

    void BeginRaisingRightWall() {
        raisingRightWall = true;
    }

    public void Begin() {
        activated = true;
        mainCamera.position = new Vector3(cameraLockX, mainCamera.position.y, mainCamera.position.z);
        hearts.gameObject.SetActive(true);
        BeginLoweringLeftWall();
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
