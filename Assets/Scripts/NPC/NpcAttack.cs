using UnityEngine;

public class NpcAttack : MonoBehaviour
{
    public int damage = 10; // Amount of damage the NPC inflicts

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the NPC collides with the player
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            // Calculate the direction of the hit
            // Inflict damage to the player and pass the hit direction
            player.TakeDamage(damage);
        }
    }
}
