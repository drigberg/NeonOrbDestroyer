using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float secondsToDestroy = 1f;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() {
        secondsToDestroy -= Time.deltaTime;
        if (secondsToDestroy <= 0f) {
            Destroy(gameObject);
        }
    }
}
