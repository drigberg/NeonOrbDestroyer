using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenCollider : MonoBehaviour
{
    public RavenMovement raven;
    void ExplodeListener(SendExplodeArgs sendExplodeArgs) {
        raven.ExplodeListener(sendExplodeArgs);
    }
}
