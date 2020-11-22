using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonController : MonoBehaviour {
    public int index;
    [SerializeField] public bool keyDown;
    [SerializeField] public int maxIndex;
    public AudioSource audioSource;
    public bool isEnabled = true;

    void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    public void Enable() {
        isEnabled = true;
    }

    public void Disable() {
        isEnabled = false;
    }

    // Update is called once per frame
    void Update () {
        // don't scroll through options if disabled or if there's only one option
        if (!isEnabled || maxIndex == 0) {
            return;
        }

        if (Input.GetAxis("Vertical") == 0) {
            keyDown = false;
            return;
        }

        if (!keyDown){
            if (Input.GetAxis("Vertical") < 0) {
                if (index < maxIndex){
                    index++;
                } else {
                    index = 0;
                }
            } else if (Input.GetAxis("Vertical") > 0) {
                if (index > 0){
                    index --;
                } else {
                    index = maxIndex;
                }
            }
            keyDown = true;
        }
    }
}
