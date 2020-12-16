using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGameEnd : MonoBehaviour
{
    public MenuController menuController;

    void ExplodeListener(SendExplodeArgs sendExplodeArgs) {
        if (sendExplodeArgs.attacking) {
            menuController.GoToMenuScene(1f);
            TriggerMove triggerMove = GetComponent<TriggerMove>();
            triggerMove.HideSelfAndExplode();
        }
    }
}
