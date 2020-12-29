using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header ("Audio")]
    public AudioClip music;
    public bool autoplay;
    public float delayTime = 0.5f;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = music;
        if (autoplay) {
            Play();
        }
    }

    public void Pause() {
        audioSource.Pause();
    }

    public void Stop() {
        audioSource.Stop();
    }

    public void Play() {
        if (!GameSettings.muted) {
            audioSource.Play();
        }
    }

    public void PlayAfterDelay() {
        StartCoroutine("_PlayAfterDelay");
    }

    private IEnumerator _PlayAfterDelay() {
        yield return new WaitForSeconds(delayTime);
        Play();
    }
}
