using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowGauge : MonoBehaviour
{
    public Arena arena;
    public float glowRequired;
    private float glow = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddGlow(float glowToAdd) {
        glow += glowToAdd;
        if (glow >= glowRequired) {

        }
    }
}
