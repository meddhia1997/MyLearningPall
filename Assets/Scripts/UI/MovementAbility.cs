using UnityEngine;

public class MovementAbility : MonoBehaviour
{
    public GameObject objectToToggleVisibility; // Assign the object to toggle visibility

    void Start()
    {
        PlayerPrefs.SetInt("flag", 1);
    }

    void Update()
    {    Debug.Log("Flag set to 1: " + PlayerPrefs.GetInt("flag")); // Debug the value of flag

        // Check the value of the PlayerPrefs variable "flag"
        // If it's equal to 0, set the visibility of objectToToggleVisibility to false
        if (PlayerPrefs.GetInt("flag") == 0)
        {
            SetObjectVisibility(false);
        }
        // If it's equal to 1, set the visibility of objectToToggleVisibility to true
        else if (PlayerPrefs.GetInt("flag") == 1)
        {                  PlayerPrefs.SetInt("flag", 1);

            SetObjectVisibility(true);
        }
    }

    // Method to set the visibility of the object
    void SetObjectVisibility(bool isVisible)
    {
        if (objectToToggleVisibility != null)
        {
            objectToToggleVisibility.SetActive(isVisible);
        }
        else
        {
            Debug.LogWarning("No object assigned to toggle visibility.");
        }
    }
}
