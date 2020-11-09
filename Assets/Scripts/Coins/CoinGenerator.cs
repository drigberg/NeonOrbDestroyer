using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public float generateProb = 0.005f;
    public Coin CoinPrefab;
    public float maxSpawnX = 15f;
    public float spawnHeight = 30f;

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0f, 1f) < generateProb) {
            float spawnX = Random.Range(maxSpawnX * -1f, maxSpawnX);
            Vector3 spawnPoint = new Vector3(spawnX, spawnHeight, 0f);
            Coin coin = Instantiate(CoinPrefab, spawnPoint, new Quaternion());
        }
    }
}
