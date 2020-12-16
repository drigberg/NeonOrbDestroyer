using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArenaBegin : MonoBehaviour
{
    public Arena arena;
    public int glow = 25;

    void ExplodeListener(SendExplodeArgs sendExplodeArgs) {
        if (sendExplodeArgs.attacking) {
            sendExplodeArgs.glow = glow;
            arena.Begin();
            TriggerMove triggerMove = GetComponent<TriggerMove>();
            triggerMove.DestroySelf(true);
        }
    }
}
