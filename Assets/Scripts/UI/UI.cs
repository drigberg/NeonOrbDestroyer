using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text countdownText;

    public void SetCountdown(int seconds) {
        countdownText.gameObject.SetActive(true);
        countdownText.text = seconds.ToString();
    }
    public void HideCountdown() {
        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
    }
    public void CountDownZero() {
        countdownText.text = "GO";
    }
}
