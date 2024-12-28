using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 4f; // Maximum distance the bullet can travel before being destroyed
    public int damage = 10; // Amount of damage the bullet deals
    private Vector2 targetDirection;
    private Vector2 startPosition;

    public void SetTarget(Transform target)
    {
        targetDirection = (target.position - transform.position).normalized;
    }

    void Start()
    {
        // Set the initial position of the bullet
        startPosition = transform.position;
        
        // Set the bullet's velocity in the direction of the target
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = targetDirection * speed;
    }

    void Update()
    {
        // Check the distance traveled by the bullet
        float distanceTravelled = Vector2.Distance(startPosition, transform.position);
        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet collided with an enemy by tag
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hey am here now ");
            Npc enemy = collision.gameObject.GetComponent<Npc>();
            if (enemy != null)
            {
                // Apply damage to the enemy
                enemy.TakeDamage(damage);
            }

            // Destroy the bullet
            Destroy(gameObject);
        }
    }
    public void IncreaseSpeed()
    {
        speed += 1f; // Example increase speed
        Debug.Log("Speed increased to: " + speed);
        
        // Update the bullet's velocity with the new speed
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = targetDirection * speed;
    }

    // Method to increase damage
    public void IncreaseDamage()
    {
        damage += 5; // Example increase damage
        Debug.Log("Damage increased to: " + damage);
    }
      
}
