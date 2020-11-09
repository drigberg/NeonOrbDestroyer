﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{
    public float generateProb = 0.005f;
    public MeteorMove MeteorPrefab;
    public float maxSpawnX = 15f;
    public float spawnHeight = 30f;
    public float difficultyStep = 0.00001f;

    // Update is called once per frame
    void Update()
    {
        generateProb += difficultyStep * Time.deltaTime;
        if (Random.Range(0f, 1f) < generateProb) {
            float spawnX = Random.Range(maxSpawnX * -1f, maxSpawnX);
            Vector3 spawnPoint = new Vector3(spawnX, spawnHeight, 0f);
            MeteorMove meteor = Instantiate(MeteorPrefab, spawnPoint, new Quaternion());
        }
    }
}
