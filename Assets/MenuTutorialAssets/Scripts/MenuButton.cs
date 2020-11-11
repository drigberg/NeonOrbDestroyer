using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] public MenuButtonController menuButtonController;
    [SerializeField] public Animator animator;
    [SerializeField] public AnimatorFunctions animatorFunctions;
    [SerializeField] public int thisIndex;
    public enum Action {NEW_GAME_EASY, NEW_GAME_HARD, RETURN_TO_MENU};
    public Action action;

    // Update is called once per frame
    void Update() {
        if (menuButtonController.isDisabled) {
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
