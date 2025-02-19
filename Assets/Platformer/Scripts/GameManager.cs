using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private int timeLeft;

    // Update is called once per frame
    void Update()
    {
        timeLeft = 300 - (int)Time.time;
        timerText.text = $"Time: {timeLeft}";
    }
}
