using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour {
    public string[] messages;
    public Text text;
    public string defaultText;
    private int currentMessageIndex = -1;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void NextMessage() {
        currentMessageIndex = currentMessageIndex + 1;
        if (currentMessageIndex < messages.Length) {
            text.text = messages[currentMessageIndex];
        } else {
            Reset();
        }
    }

    void Reset() {
        currentMessageIndex = -1;
        text.text = "";
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            if (currentMessageIndex == -1) {
                text.text = defaultText;
            } else {
                text.text = messages[currentMessageIndex];
            }
            PlayerController player = collider.GetComponent<PlayerController>();
            player.activeNPC = this;
        }
    }

    void OnTriggerExit(Collider collider) {
        if (collider.tag == "Player") {
            text.text = "";
            PlayerController player = collider.GetComponent<PlayerController>();
            player.activeNPC = null;
        }
    }
}
