using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public bool isEnabled = false;

    public Coin coinPrefab;
    public float coinGenerateProb = 0.001f;

    public RavenMovement RavenPrefab;
    public float ravenGenerateProb = 0.005f;

    public Transform centerSpawnPoint;
    public float spawnXSpread = 15f;

    void Start() {}

    public void Reset() {
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
            Vector3 spawnPoint = GetSpawnPoint();
            Coin coin = Instantiate(coinPrefab, spawnPoint, new Quaternion());
            coin.transform.parent = gameObject.transform;
        }
    }

    void GenerateRaven() {
        if (Random.Range(0f, 1f) < ravenGenerateProb) {
            Vector3 spawnPoint = GetSpawnPoint();
            RavenMovement raven = Instantiate(RavenPrefab, spawnPoint, new Quaternion());
            raven.transform.parent = gameObject.transform;
        }
    }

    Vector3 GetSpawnPoint() {
        Vector3 spawnPoint = new Vector3(
            Random.Range(centerSpawnPoint.position.x - spawnXSpread, centerSpawnPoint.position.x + spawnXSpread),
            centerSpawnPoint.position.y,
            0f);

        return spawnPoint;
    }
}
