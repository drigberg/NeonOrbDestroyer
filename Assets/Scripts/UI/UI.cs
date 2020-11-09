using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI pointsText;
    public TMPro.TextMeshProUGUI countdownText;

    public void SetPoints(int points) {
        pointsText.gameObject.SetActive(true);
        pointsText.SetText(points.ToString());
    }

    public void SetCountdown(int seconds) {
        countdownText.gameObject.SetActive(true);
        countdownText.SetText(seconds.ToString());
    }
    public void HideCountdown() {
        countdownText.SetText("");
        countdownText.gameObject.SetActive(false);
    }
    public void CountDownZero() {
        countdownText.SetText("GO");
    }
}
