using UnityEngine;
using UnityEngine.UI;

public class WeaponParameters : MonoBehaviour
{
    public HomingBullet homingBullet; // Reference to the bullet script where parameters will be updated
    public GameObject objectToDestroy; // Reference to the object that should be destroyed
    private float customDeltaTime;

    // UI Buttons to adjust parameters
    public Button fireRateButton;
    public Button damageButton;

    void Start()
    {
         Time.timeScale=0;
        customDeltaTime = Time.unscaledDeltaTime;
        fireRateButton.onClick.AddListener(IncreaseFireRate);
        damageButton.onClick.AddListener(IncreaseDamage);
    }
    void Update()
    {
        customDeltaTime = Time.unscaledDeltaTime;
    }

    void IncreaseFireRate()
    {
        homingBullet.IncreaseSpeed();
        DestroyAssignedPrefab(); // Destroy the assigned prefab after increasing the fire rate
    }

    void IncreaseDamage()
    {
        homingBullet.IncreaseDamage();
        DestroyAssignedPrefab(); // Destroy the assigned prefab after increasing the damage
    }

    void DestroyAssignedPrefab()
    {
        Time.timeScale=1;
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
            Debug.Log("Assigned prefab destroyed.");
        }
        else
        {
            Debug.LogWarning("No assigned prefab set to destroy.");
        }
    }
}
