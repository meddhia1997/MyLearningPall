using UnityEngine;

public class BulletSound : MonoBehaviour
{
    public AudioClip spawnSound; // Sound clip for bullet spawn
    private AudioSource audioSource;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if not already present
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Play spawn sound if assigned
        if (spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }
    }

    void OnDisable()
    {
        // Stop audio when bullet is disabled or destroyed
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
