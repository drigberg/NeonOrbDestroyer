using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public bool isEnabled = false;

    public Coin coinPrefab;
    public float coinGenerateProb = 0.001f;

    public RavenMovement RavenPrefab;
    public float ravenGenerateProbInitial = 0.005f;
    public float ravenProbStep = 0.00001f;
    private float ravenGenerateProb;

    public float maxSpawnX = 15f;
    public float spawnHeight = 30f;

    void Start() {}

    public void Reset() {
        ravenGenerateProb = ravenGenerateProbInitial;
        isEnabled = false;
    }

    public void Enable() {
        isEnabled = true;
    }

    public void Disable() {
        isEnabled = false;
        BroadcastMessage("DestroySelf", true, SendMessageOptions.DontRequireReceiver);
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled) {
            GenerateCoin();
            GenerateRaven();
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

    void GenerateRaven() {
        ravenGenerateProb += ravenProbStep * Time.deltaTime;
        if (Random.Range(0f, 1f) < ravenGenerateProb) {
            float spawnX = Random.Range(maxSpawnX * -1f, maxSpawnX);
            Vector3 spawnPoint = new Vector3(spawnX, spawnHeight, 0f);
            RavenMovement raven = Instantiate(RavenPrefab, spawnPoint, new Quaternion());
            raven.transform.parent = gameObject.transform;
        }
    }
}
