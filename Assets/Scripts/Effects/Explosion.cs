using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float secondsToDestroy = 2f;
    public AudioClip soundEffect;

    // Start is called before the first frame update
    void Start() {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(soundEffect);
    }

    // Update is called once per frame
    void Update() {
        secondsToDestroy -= Time.deltaTime;
        if (secondsToDestroy <= 0f) {
            Destroy(gameObject);
        }
    }
}
