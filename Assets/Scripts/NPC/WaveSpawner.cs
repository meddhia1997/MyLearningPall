using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<GameObject> assignedPrefabs = new List<GameObject>(); // List of prefabs to spawn after each wave ends
    public Tilemap tilemap;
    public Vector3Int[] spawnPositions; // Example spawn positions on the Tilemap

    private int currentWave = 1;
    private int enemiesPerWave = 20;
    private int totalWaves = 2;
    private int prefabIndex = 0; // Index to keep track of which prefab to spawn
    public Npc npc;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave <= totalWaves)
        {
            yield return StartCoroutine(SpawnWave());
            yield return new WaitForSeconds(1.5f); // Delay between waves

            SpawnAssignedPrefab();
            currentWave++;
            totalWaves++;
            npc.maxHealth += 10;

            yield return new WaitForSeconds(2f); // Delay between waves
        }
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            Vector3Int spawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyPrefab, tilemap.CellToWorld(spawnPosition), Quaternion.identity);
            yield return new WaitForSeconds(1f); // Delay between each enemy spawn
        }

        // Wait until all enemies of this wave are gone
        while (GameObject.FindWithTag("Enemy") != null)
        {
            yield return null;
        }
    }

    void SpawnAssignedPrefab()
    {
        if (assignedPrefabs.Count == 0)
        {
            Debug.LogWarning("No assigned prefabs set to spawn after waves.");
            return;
        }

        // Spawn the prefab at a predefined position or a random spawn position
        Vector3Int spawnPosition = GetRandomSpawnPosition(); // You can adjust this as needed
        Instantiate(assignedPrefabs[prefabIndex], tilemap.CellToWorld(spawnPosition), Quaternion.identity);

        // Update the index to the next prefab, wrapping around if necessary
        prefabIndex = (prefabIndex + 1) % assignedPrefabs.Count;
    }

    Vector3Int GetRandomSpawnPosition()
    {
        int randomIndex = Random.Range(0, spawnPositions.Length);
        return spawnPositions[randomIndex];
    }
}
