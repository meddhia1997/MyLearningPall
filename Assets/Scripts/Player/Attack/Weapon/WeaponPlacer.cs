using UnityEngine;

public class WeaponPlacer : MonoBehaviour
{
    public WeaponSelector weaponSelector;
    public Transform[] chosenLocations; // Array of potential locations for instantiation

    public void PlaceSelectedWeapon(int index)
    {
        if (index >= 0 && index < weaponSelector.spritePrefabs.Count)
        {
            GameObject selectedPrefab = weaponSelector.spritePrefabs[index].weaponprefab;

            if (selectedPrefab != null && chosenLocations.Length > 0)
            {
                foreach (Transform location in chosenLocations)
                {
                    // Check if the location is empty (no child objects)
                    if (location.childCount == 0)
                    {
                        // Calculate local position relative to parent
                        Vector3 localPosition = location.localPosition;
                        Quaternion localRotation = location.localRotation;

                        // Instantiate the selected prefab at the calculated local position and rotation
                        GameObject weaponInstance = Instantiate(selectedPrefab, location.parent);
                        weaponInstance.transform.localPosition = localPosition;
                        weaponInstance.transform.localRotation = localRotation;

                        Debug.Log($"Placed weapon prefab: {selectedPrefab.name} at local position {localPosition} relative to parent.");
                        return; // Exit after placing the weapon
                    }
                }

                Debug.LogWarning("All chosen locations are occupied. Cannot place weapon prefab.");
            }
            else
            {
                Debug.LogWarning("No weapon prefab selected or no chosen locations set.");
            }
        }
        else
        {
            Debug.LogWarning($"Invalid index {index} for weapon selection.");
        }
    }
}
