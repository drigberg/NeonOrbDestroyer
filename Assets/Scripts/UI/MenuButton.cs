﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    public MenuController menuController;
    [SerializeField] public MenuButtonController menuButtonController;
    [SerializeField] public Animator animator;
    [SerializeField] public AnimatorFunctions animatorFunctions;
    [SerializeField] public int thisIndex;
    public enum Action {NEW_GAME_EASY, NEW_GAME_HARD, RETURN_TO_MENU, GO_TO_SCREEN, UNPAUSE, TOGGLE_MUTE};
    public Action action;
    public Text text;

    [Header ("Go-To-Screen Options")]
    public int targetScreenIndex;

    void Start() {
        if (action == Action.TOGGLE_MUTE) {
            text.text = GameSettings.muted ? "UNMUTE" : "MUTE";
        }
    }

    // Update is called once per frame
    void Update() {
        if (!menuButtonController.isEnabled) {
            animator.SetBool("selected", false);
            animator.SetBool("pressed", false);
            animatorFunctions.disableOnce = true;
            return;
        }

        if (menuButtonController.index == thisIndex) {
            animator.SetBool("selected", true);
            // we don't allow submit on Space because that's the player might be jumping when the game ends,
            // and then we immediately get sent to the main menu
            if (Input.GetKeyDown(KeyCode.Return)) {
                // the mute button needs to take effect before the "submit" sound is played
                if (action == Action.TOGGLE_MUTE) {
                    GameSettings.ToggleMute();
                }
                animator.SetBool ("pressed", true);
                OnSubmit();
            } else if (animator.GetBool ("pressed")) {
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        } else {
            animator.SetBool("selected", false);
        }
    }

    void OnSubmit() {
        StartCoroutine(OnSubmitAsync());
    }

    IEnumerator OnSubmitAsync()
    {
        menuButtonController.Disable();
        // this timing method works while Time.timeScale is 0!
        float pauseEndTime = Time.realtimeSinceStartup + 0.5f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }

        if (action == Action.NEW_GAME_EASY) {
            menuController.GoToArenaScene(false);
        } else if (action == Action.NEW_GAME_HARD) {
            menuController.GoToArenaScene(true);
        } else if (action == Action.RETURN_TO_MENU) {
            menuController.GoToMenuScene(0f);
        } else if (action == Action.GO_TO_SCREEN) {
            menuController.ShowScreenByIndex(targetScreenIndex);
        } else if (action == Action.UNPAUSE) {
            menuController.DisableAllScreens();
            Time.timeScale = 1f;
        } else if (action == Action.TOGGLE_MUTE) {
            // muting is handled in Update()
            text.text = GameSettings.muted ? "UNMUTE" : "MUTE";
        }
        menuButtonController.Enable();
    }
}
