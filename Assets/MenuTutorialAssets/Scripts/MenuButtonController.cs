﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour {

    // Use this for initialization
    public int index;
    [SerializeField] bool keyDown;
    [SerializeField] int maxIndex;
    public AudioSource audioSource;
    public bool isDisabled;

    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        if (isDisabled) {
            return;
        }

        if (Input.GetAxis("Vertical") == 0) {
            keyDown = false;
            return;
        }

        if (!keyDown){
            if (Input.GetAxis("Vertical") < 0) {
                if (index < maxIndex){
                    index++;
                } else {
                    index = 0;
                }
            } else if (Input.GetAxis("Vertical") > 0) {
                if (index > 0){
                    index --;
                } else {
                    index = maxIndex;
                }
            }
            keyDown = true;
        }
    }

    public void StartGame(bool hardMode) {
        isDisabled = true;
        // set hard mode in static property of GameSettings class
        StartCoroutine(LoadArenaAsync());
    }

    IEnumerator LoadArenaAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Arena");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}