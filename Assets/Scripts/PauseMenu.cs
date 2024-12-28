using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public Button destroyButton; // Reference to the button that will destroy the prefab
    public Button navigateButton; // Reference to the button that will navigate to the MainScreen
    public GameObject prefabToDestroy; // The prefab to be destroyed
    public string mainScreenSceneName = "MainScreen"; // The name of the MainScreen scene

    private float customDeltaTime;

    void Start()
    {
        // Pause the game
        Time.timeScale = 0;
        customDeltaTime = Time.unscaledDeltaTime; // Initialize custom deltaTime

        // Assign button listeners
        if (destroyButton != null)
        {
            destroyButton.onClick.AddListener(DestroyPrefab);
        }
        else
        {
            Debug.LogError("Destroy Button is not assigned to ButtonActions script.");
        }

        if (navigateButton != null)
        {
            navigateButton.onClick.AddListener(NavigateToMainScreen);
        }
        else
        {
            Debug.LogError("Navigate Button is not assigned to ButtonActions script.");
        }
    }

    void Update()
    {
        customDeltaTime = Time.unscaledDeltaTime;
    }

    void DestroyPrefab()
    {
        // Restore the time scale before destroying the prefab
        Time.timeScale = 1;

        if (prefabToDestroy != null)
        {
            Destroy(prefabToDestroy);
            Debug.Log("Prefab destroyed: " + prefabToDestroy.name);
        }
        else
        {
            Debug.LogError("Prefab to destroy is not assigned.");
        }
    }

    void NavigateToMainScreen()
    {
        // Restore the time scale before navigating to the MainScreen
        Time.timeScale = 1;
        SceneManager.LoadScene(mainScreenSceneName);
    }
}
