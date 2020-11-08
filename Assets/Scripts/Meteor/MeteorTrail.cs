using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorTrail : MonoBehaviour
{
    public ParticleSystem particles;
    public float secondsToDestroy = 2f;
    private bool smoking;

    // Start is called before the first frame update
    void Start()
    {
        smoking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!smoking) {
            secondsToDestroy -= Time.deltaTime;
            if (secondsToDestroy <= 0f) {
                Destroy(gameObject);
            }
        }
    }

    public void StopSmoking() {
        smoking = false;
        particles.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }
}
