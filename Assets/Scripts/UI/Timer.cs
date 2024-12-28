using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeText; // Reference to the UI Text component where time will be displayed

    private float startTime; // Time when the counter started
    private bool isCounting; // Flag to track if the counter is currently counting

    public string time;

    void Start()
    {
        startTime = Time.time; // Record the start time
        isCounting = true; // Start counting
    }

    void Update()
    {
        if (isCounting)
        {
            // Calculate elapsed time
            float elapsedTime = Time.time - startTime;

            // Format minutes and seconds
            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);

            // Update UI text to display formatted time
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            time = timeText.text;

        }
    }
}
