using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    public enum Action {NEW_GAME_EASY, NEW_GAME_HARD, RETURN_TO_MENU};
    public Action action;

    // Update is called once per frame
    void Update() {
        if (menuButtonController.isDisabled) {
            animatorFunctions.disableOnce = true;
            return;
        }

        if (menuButtonController.index == thisIndex) {
            animator.SetBool("selected", true);
            if (Input.GetAxis("Submit") == 1) {
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
        if (action == Action.NEW_GAME_EASY) {
            menuButtonController.StartGame(false);
        } else if (action == Action.NEW_GAME_HARD) {
            menuButtonController.StartGame(true);
        } else if (action == Action.RETURN_TO_MENU) {
            menuButtonController.ReturnToMenu();
        }
    }
}
