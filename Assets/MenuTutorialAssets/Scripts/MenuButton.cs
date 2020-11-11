using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public MenuController menuController;
    [SerializeField] public MenuButtonController menuButtonController;
    [SerializeField] public Animator animator;
    [SerializeField] public AnimatorFunctions animatorFunctions;
    [SerializeField] public int thisIndex;
    public enum Action {NEW_GAME_EASY, NEW_GAME_HARD, RETURN_TO_MENU, GO_TO_SCREEN};
    public Action action;

    [Header ("Go-To-Screen Options")]
    public int targetScreenIndex;


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
        // wait for the button animation
        yield return new WaitForSeconds(0.5f);
        if (action == Action.NEW_GAME_EASY) {
            menuController.GoToArenaScene(false);
        } else if (action == Action.NEW_GAME_HARD) {
            menuController.GoToArenaScene(true);
        } else if (action == Action.RETURN_TO_MENU) {
            menuController.GoToMenuScene();
        } else if (action == Action.GO_TO_SCREEN) {
            menuController.ShowScreenByIndex(targetScreenIndex);
        }
        menuButtonController.Enable();
    }
}
