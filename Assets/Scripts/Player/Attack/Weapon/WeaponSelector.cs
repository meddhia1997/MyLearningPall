using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    [System.Serializable]
    public class SpritePrefabData
    {
        public GameObject prefab;
        public Sprite sprite;
        public GameObject weaponprefab;
    }

    public List<SpritePrefabData> spritePrefabs = new List<SpritePrefabData>();
    public Transform spawnArea; // The area where buttons will be spawned
    public WeaponPlacer weaponPlacer; // Reference to the WeaponPlacer script

    void Start()
    {
        SpawnSprites();
    }

    void SpawnSprites()
    {
        foreach (var data in spritePrefabs)
        {
            GameObject buttonPrefab = data.prefab;
            Sprite sprite = data.sprite;

            // Instantiate the prefab
            GameObject buttonInstance = Instantiate(buttonPrefab, spawnArea);

            // Get the Image component from the instantiated prefab
            Image imageComponent = buttonInstance.GetComponent<Image>();
            if (imageComponent != null)
            {
                // Assign the sprite to the Image component
                imageComponent.sprite = sprite;

                // Add an onClick listener to the button to handle clicks
                Button button = buttonInstance.GetComponent<Button>();
                if (button != null)
                {
                    int index = spritePrefabs.IndexOf(data); // Capture the current index
                    button.onClick.AddListener(() => OnButtonClick(index));
                }
                else
                {
                    Debug.LogWarning($"Prefab {buttonPrefab.name} is missing a Button component.");
                }
            }
            else
            {
                Debug.LogWarning($"Prefab {buttonPrefab.name} is missing an Image component.");
            }
        }
    }

    void OnButtonClick(int index)
    {
        if (weaponPlacer != null)
        {
            weaponPlacer.PlaceSelectedWeapon(index);
        }
        else
        {
            Debug.LogWarning("WeaponPlacer reference not set in WeaponSelector.");
        }
    }
}
