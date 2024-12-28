using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Button spawnButton; // Reference to the UI Button component
    public GameObject prefabToSpawn; // The prefab to spawn
    public Transform spawnPoint; // The position where the prefab will be spawned

    void Start()
    {
        // Ensure the button is assigned and add a listener to call SpawnObject() when clicked
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(SpawnObject);
        }
        else
        {
            Debug.LogError("Spawn Button is not assigned to Spawner script.");
        }

        // Ensure the prefab to spawn is assigned
        if (prefabToSpawn == null)
        {
            Debug.LogError("Prefab to spawn is not assigned to Spawner script.");
        }

        // Ensure the spawn point is assigned
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is not assigned to Spawner script.");
        }
    }

    // Method to spawn the prefab at the specified spawn point
    void SpawnObject()
    {
        if (prefabToSpawn != null && spawnPoint != null)
        {
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Spawned prefab at position: " + spawnPoint.position);
        }
        else
        {
            Debug.LogError("Cannot spawn object. Prefab or spawn point is not assigned.");
        }
    }
}
