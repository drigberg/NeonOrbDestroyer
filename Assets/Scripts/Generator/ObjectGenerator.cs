using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    public Coin CoinPrefab;
    public float coinGenerateProb = 0.001f;

    public MeteorMove MeteorPrefab;
    public float meteorGenerateProb = 0.005f;
    public float meteorProbStep = 0.00001f;

    public float maxSpawnX = 15f;
    public float spawnHeight = 30f;

    // Update is called once per frame
    void Update()
    {
        GenerateCoin();
        GenerateMeteor();
    }

    void GenerateCoin() {
        if (Random.Range(0f, 1f) < coinGenerateProb) {
            float spawnX = Random.Range(maxSpawnX * -1f, maxSpawnX);
            Vector3 spawnPoint = new Vector3(spawnX, spawnHeight, 0f);
            Coin coin = Instantiate(CoinPrefab, spawnPoint, new Quaternion());
        }
    }

    void GenerateMeteor() {
        meteorGenerateProb += meteorProbStep * Time.deltaTime;
        if (Random.Range(0f, 1f) < meteorGenerateProb) {
            float spawnX = Random.Range(maxSpawnX * -1f, maxSpawnX);
            Vector3 spawnPoint = new Vector3(spawnX, spawnHeight, 0f);
            MeteorMove meteor = Instantiate(MeteorPrefab, spawnPoint, new Quaternion());
        }
    }
}
