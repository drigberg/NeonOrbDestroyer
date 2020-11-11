using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField] MenuButtonController menuButtonController;
    [SerializeField] Animator animator;
    [SerializeField] AnimatorFunctions animatorFunctions;
    [SerializeField] int thisIndex;
    public bool hardMode;

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
                menuButtonController.StartGame(hardMode);
            } else if (animator.GetBool ("pressed")) {
                animator.SetBool("pressed", false);
                animatorFunctions.disableOnce = true;
            }
        } else {
            animator.SetBool("selected", false);
        }
    }
}
