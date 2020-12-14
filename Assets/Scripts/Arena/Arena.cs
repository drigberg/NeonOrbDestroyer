using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public PlayerController player;
    public Hearts hearts;
    public ObjectGenerator objectGenerator;
    public GlowGauge glowGauge;

    // Start is called before the first frame update
    void Start() {}

    void OnEnter() {
        objectGenerator.Activate();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
