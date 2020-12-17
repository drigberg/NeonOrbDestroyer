using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowGauge : MonoBehaviour
{
    public Arena arena;
    public float glowRequired;
    private float glow = 0f;
    public MaterialGlowControl[] materialGlowControls;

    public void AddGlow(float glowToAdd) {
        glow += glowToAdd;
        if (glow > glowRequired) {
            glow = glowRequired;
        }

        foreach (MaterialGlowControl materialGlowControl in materialGlowControls) {
            materialGlowControl.SetBrightness(glow / glowRequired);
        }

        if (glow == glowRequired) {
            arena.End();
            glow = 0f;
        }
    }
}
