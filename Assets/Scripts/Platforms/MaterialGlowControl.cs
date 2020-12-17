using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialGlowControl : MonoBehaviour
{
    public Material material;

    private Color initialColor;
    private Color initialEmissionColor;

    void Start()
    {
        // NOTE: it would be neat to create a new instance of each color on platform start, and use that copy so that
        // we never have bugs from switching scenes, etc. This would also let us reuse colors between rooms safely,
        // if we want them to be updated at different stages. Otherwise, we could always just be really careful when
        // using manual copies of colors (eg: "Arena 1 Cyan" vs "Arena 2 Cyan")

        Color color = material.GetColor("_Color");
        initialColor = new Color(color.r, color.g, color.b);

        Color emissionColor = material.GetColor("_EmissionColor");
        initialEmissionColor = new Color(emissionColor.r, emissionColor.g, emissionColor.b);

        material.SetColor("_Color", new Color(0f, 0f, 0f));
        material.SetColor("_EmissionColor", new Color(0f, 0f, 0f));
    }

    public void SetBrightness(float brightness) {
        material.SetColor("_Color", new Color(initialColor.r * brightness, initialColor.g * brightness, initialColor.b * brightness));
        material.SetColor("_EmissionColor", new Color(initialEmissionColor.r * brightness, initialEmissionColor.g * brightness, initialEmissionColor.b * brightness));
    }

    void OnApplicationQuit() {
        material.SetColor("_Color", initialColor);
        material.SetColor("_EmissionColor", initialEmissionColor);
    }
}
