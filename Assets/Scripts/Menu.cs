using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Text timeText; // Assign this in the Unity Editor
    public Button navigateButton; // Assign this in the Unity Editor
        private float customDeltaTime;


    void Start()
    {   Time.timeScale=0;

        customDeltaTime = Time.unscaledDeltaTime;
        navigateButton.onClick.AddListener(NavigateToMainScreen);

        // Display the time when the script starts
        DisplayTime();
    }
 void Update()
{
    customDeltaTime = Time.unscaledDeltaTime;
}
    // Function to display the time stored in PlayerPrefs in the assigned Text component
    public void DisplayTime()
    {
        // Get the time value from PlayerPrefs (assuming the key is "Time")
        string time = PlayerPrefs.GetString("Time", "00:00"); // Default to "00:00" if the key doesn't exist

        // Display the time in the Text component
        timeText.text = time;
    }

    // Function to navigate to the "MainScreen" scene
    public void NavigateToMainScreen()
    {
        Time.timeScale=1;
        SceneManager.LoadScene("MainScreen");
    }
}
