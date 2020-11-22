using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings : object
{
    public static bool muted = false;

    public static bool ToggleMute() {
        muted = !muted;
        return muted;
    }
}
