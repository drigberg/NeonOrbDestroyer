using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public UI ui;
    public int countdownSeconds = 3;
    private bool isEnabled = false;

    public Coin coinPrefab;
    public float coinGenerateProb = 0.001f;

    public PinkEnemyMove PinkEnemyPrefab;
    public float pinkEnemyGenerateProbInitial = 0.005f;
    public float pinkEnemyProbStep = 0.00001f;
    private float pinkEnemyGenerateProb;

    public float maxSpawnX = 15f;
    public float spawnHeight = 30f;

    void Start() {
        StartCoroutine("CountdownToStart");
    }

    void Reset() {
        pinkEnemyGenerateProb = pinkEnemyGenerateProbInitial;
        isEnabled = false;
    }

    void Enable() {
        isEnabled = true;
    }

    public void Disable() {
        isEnabled = false;
        BroadcastMessage("DestroySelf", true, SendMessageOptions.DontRequireReceiver);
    }

    private IEnumerator CountdownToStart() {
        Reset();
        int steps = 0;
        while (steps < countdownSeconds) {
            ui.SetCountdown(countdownSeconds - steps);
            yield return new WaitForSeconds(1);
            steps += 1;
        }
        ui.CountDownZero();
        yield return new WaitForSeconds(1);
        ui.HideCountdown();
        Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled) {
            GenerateCoin();
            GeneratePinkEnemy();
        }
    }

    void GenerateCoin() {
        if (Random.Range(0f, 1f) < coinGenerateProb) {
            float spawnX = Random.Range(maxSpawnX * -1f, maxSpawnX);
            Vector3 spawnPoint = new Vector3(spawnX, spawnHeight, 0f);
            Coin coin = Instantiate(coinPrefab, spawnPoint, new Quaternion());
            coin.transform.parent = gameObject.transform;
        }
    }

    void GeneratePinkEnemy() {
        pinkEnemyGenerateProb += pinkEnemyProbStep * Time.deltaTime;
        if (Random.Range(0f, 1f) < pinkEnemyGenerateProb) {
            float spawnX = Random.Range(maxSpawnX * -1f, maxSpawnX);
            Vector3 spawnPoint = new Vector3(spawnX, spawnHeight, 0f);
            PinkEnemyMove pinkEnemy = Instantiate(PinkEnemyPrefab, spawnPoint, new Quaternion());
            pinkEnemy.transform.parent = gameObject.transform;
        }
    }
}
