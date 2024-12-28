using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player : MonoBehaviour
{
        public GameObject assignedPrefab; // List of prefabs to spawn after each wave ends

    public int maxHealth = 100; // The maximum health of the player
    public int currentHealth; // The current health of the player
    public int attack = 10; // The attack power of the player
    public int defense = 5; // The defense power of the player
    public Animator animator; // Reference to the Animator component for triggering animations
    public float knockbackForce = 10f; // The force applied to the player when hit
public Timer timer ;

private string time ;
    public HealthBar healthBar; // Reference to the HealthBar script to update UI

    private Rigidbody2D rb; // Reference to the Rigidbody2D component for physics calculations

    void Start()
    {
        // Initialize player's health and update health bar UI
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Get the Rigidbody2D component attached to the player for applying physics
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage)
    {
        // Calculate damage after applying defense
        int damageAfterDefense = Mathf.Max(damage - defense, 0);

        // Reduce current health by the calculated damage
        currentHealth -= damageAfterDefense;

        // Ensure current health doesn't drop below 0
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update health bar UI to reflect the new health value
        healthBar.SetHealth(currentHealth);

        // Log a message indicating damage was taken
        Debug.Log("taking damage!");

        // Trigger the 'Hitted' animation
        animator.SetTrigger("Hitted");

        // Apply knockback effect if the player is still alive
        ApplyKnockback();

        // Check if the player's health has dropped to 0 or below
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method to handle the player's death
        }
    }

    void ApplyKnockback()
    {
        // Apply a knockback force to the player
        Vector2 knockbackDirection = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    }

    public void Heal(int amount)
    {
        // Increase current health by the heal amount
        currentHealth += amount;

        // Ensure current health doesn't exceed max health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update health bar UI to reflect the new health value
        healthBar.SetHealth(currentHealth);
    }

    void Die()
    {
        // Handle the player's death (e.g., play a death animation, disable player controls)
        Debug.Log("Player died!");
        animator.SetTrigger("Die"); // Trigger the 'Die' animation (assuming you have one)
        
        // Get the time from the timer
        time = timer.time;
        Debug.Log("Time: " + time);
PlayerPrefs.SetString("Time", time); // This can be done from another script

        // Send time to the score route using UnityWebRequest
        StartCoroutine(SendTimeToScore());
        SpawnAssignedPrefab();
    }

    IEnumerator SendTimeToScore()
    {
        string url = $"localhost:3000/score/{time}";

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, ""))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to send time to score route: " + www.error);
            }
            else
            {
                Debug.Log("Time sent successfully to score route.");
            }
        }
    }
    public void AddMaxHealth(int amount)
    {
        // Increase the player's max health
        maxHealth += amount;

        // Optionally heal the player to full health when max health increases
        currentHealth = maxHealth;

        // Update the health bar UI to reflect the new max health
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }


  public void SpawnAssignedPrefab()
    {
      

        // Spawn the prefab at a predefined position or a random spawn position
        Instantiate(assignedPrefab);

        // Update the index to the next prefab, wrapping around if necessary
    }



}