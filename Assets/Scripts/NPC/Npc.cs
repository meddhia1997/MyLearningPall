using UnityEngine;

public class Npc : MonoBehaviour
{
    public int maxHealth = 100; // The maximum health of the NPC
    public int currentHealth; // The current health of the NPC
    public int attack = 10; // The attack power of the NPC
    public int defense = 5; // The defense power of the NPC

    public HealthBar healthBar; // Reference to the HealthBar script to update UI

    private Rigidbody2D rb; // Reference to the Rigidbody2D component for physics calculations

    void Start()
    {
        // Initialize NPC's health and update health bar UI
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        // Get the Rigidbody2D component attached to the NPC for applying physics
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
        Debug.Log("NPC taking damage!");

        // Check if the NPC's health has dropped to 0 or below
        if (currentHealth <= 0)
        {
            Die(); // Call the Die method to handle the NPC's death
        }
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
        // Handle the NPC's death (e.g., play a death animation, disable NPC controls)
        Debug.Log("NPC died!");

        // Destroy the NPC game object
        Destroy(gameObject);
    }

    public void AddMaxHealth(int amount)
    {
        // Increase the NPC's max health
        maxHealth += amount;

        // Optionally heal the NPC to full health when max health increases
        currentHealth = maxHealth;

        // Update the health bar UI to reflect the new max health
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }
}
