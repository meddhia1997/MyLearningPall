using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{
    public Button logoutButton; // Assign the logout button from the scene in the Unity Editor

    void Start()
    {
        // Check if the logout button is assigned
        if (logoutButton != null)
        {
            // Add listener for the logout button click event
            logoutButton.onClick.AddListener(LogoutUser);
        }
        else
        {
            Debug.LogWarning("Logout button is not assigned!");
        }
    }

    void LogoutUser()
    {
        // Remove JWT token from PlayerPrefs
        PlayerPrefs.DeleteKey("JWTToken");

        // Load the login scene
        SceneManager.LoadScene("Login"); // Replace "Login" with the actual name of your login scene
    }
}
