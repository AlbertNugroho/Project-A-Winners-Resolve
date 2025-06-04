using TMPro;
using UnityEngine;

public class CountUp : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = (int)(elapsedTime / 60) % 60;
        int seconds = (int)(elapsedTime) % 60;
        int milliseconds = (int)((elapsedTime * 100) % 100);

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}
