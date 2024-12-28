using UnityEngine;

public class Progress : MonoBehaviour
{
    public string playerPrefsKey = "PlayerProgress"; // Key to save player progress in PlayerPrefs
    public Transform player; // Reference to the player's transform
    private string progressString; // Store progress as string

    // Initial position if no saved progress is found
    private Vector3 initialPosition = new Vector3(884.184875f, 628.339417f, 0f);

    void Start()
    {
        // Load the player progress from PlayerPrefs if it exists
        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            // Get the saved progress from PlayerPrefs
            string savedProgress = PlayerPrefs.GetString(playerPrefsKey);
            // Convert the saved progress string to a Vector2
            Vector2 newProgress = StringToVector2(savedProgress);
            // Set the player's progress based on the saved progress
            player.position = new Vector3(newProgress.x, newProgress.y, player.position.z); // Assuming z-coordinate remains unchanged

            // Debug the loaded progress
            Debug.Log("Loaded Progress: " + newProgress);
        }
        else
        {
            // Set initial position if no saved progress is found
            player.position = initialPosition;
            Debug.Log("No saved progress found. Setting player to initial position: " + initialPosition);
        }
    }

    void Update()
    {
        // Save the player's progress to PlayerPrefs
        SavePlayerProgress();
    }

    void SavePlayerProgress()
    {
        // Debug the player's current position before saving
        Debug.Log("Player Position: " + player.position);

        // Convert the player's position to a string
        progressString = Vector2ToString(new Vector2(player.position.x, player.position.y)); // Only x and y coordinates
        // Save the progress string to PlayerPrefs
        PlayerPrefs.SetString(playerPrefsKey, progressString);
        // Make sure to save PlayerPrefs to disk
        PlayerPrefs.Save();

        // Debug the saved progress
        Debug.Log("Saved Progress: " + progressString);
    }

    // Convert Vector2 to string in format "x.y"
    string Vector2ToString(Vector2 vector)
    {
        return vector.x + "." + vector.y;
    }

    // Convert string in format "x.y" to Vector2
    Vector2 StringToVector2(string str)
    {
        // Split the string by period
        string[] parts = str.Split('.');
        // Parse the parts to floats and create a Vector2
        float x = float.Parse(parts[0]);
        float y = float.Parse(parts[1]);
        return new Vector2(x, y);
    }
}
