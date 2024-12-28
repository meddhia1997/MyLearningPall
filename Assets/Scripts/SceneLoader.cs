using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Button btn; // Reference to the UI Button component
    public Button exit; // Reference to the Exit Button component
    public string levelName = "level1"; // The name of the scene you want to load

    void Start()
    {
        // Ensure the play button is assigned and add a listener to call LoadScene() when clicked
        if (btn != null)
        {
            btn.onClick.AddListener(LoadScene);
        }
        else
        {
            Debug.LogError("Play Button is not assigned to SceneLoader script.");
        }

        // Ensure the exit button is assigned and add a listener to call ExitGame() when clicked
        if (exit != null)
        {
            exit.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogError("Exit Button is not assigned to SceneLoader script.");
        }
    }

    // Method to load the specified scene
    public void LoadScene()
    {
        SceneManager.LoadScene(levelName);
    }

    // Method to exit the game
    public void ExitGame()
    {
        Debug.Log("Exit button pressed. Exiting game.");
        Application.Quit();
        
        // If running in the Unity editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
