using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject spawnPrefab; // Prefab to spawn
    public GameObject objectToDestroy; // GameObject to destroy
    public Transform spawnPoint;   // Point where the prefab will be spawned

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding GameObject has the "Player" tag
        if (collision.gameObject.CompareTag("Me"))
        {
            // Spawn the prefab at the specified spawn point
            Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);


            // Destroy objectToDestroy if assigned
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
        }
    }

}
