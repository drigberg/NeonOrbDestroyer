using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public GameObject[] screens;
    public bool startVisible = true;

    void Start() {
        DisableAllScreens();
        if (startVisible) {
            ShowScreenByIndex(0);
        }
    }

    public void DisableAllScreens() {
        for (int i = 0; i < screens.Length; i++) {
            screens[i].SetActive(false);
        }
    }

    public void ShowScreenByIndex(int i) {
        DisableAllScreens();
        screens[i].SetActive(true);
    }

    public void GoToArenaScene(bool hardMode) {
        // TODO: set hard mode in static property of GameSettings class
        StartCoroutine(LoadArenaAsync());
    }

    public void GoToMenuScene() {
        StartCoroutine(LoadMainMenuAsync());
    }

    IEnumerator LoadArenaAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Arena");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadMainMenuAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Menu");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
