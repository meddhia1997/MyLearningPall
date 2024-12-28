using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public string playerTag = "Me"; // Tag to identify the player
    public float moveSpeed = 2f; // The speed at which the enemy moves

    private Transform player; // Reference to the player's transform
    private Animator animator; // Reference to the Animator component
    private Vector3 initialScale; // Store the initial scale

    void Start()
    {
        // Find the player GameObject by tag and get its transform
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning($"No GameObject found with tag {playerTag}");
        }

        // Get the Animator component attached to the enemy
        animator = GetComponent<Animator>();

        // Store the initial scale of the enemy
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Check if the player reference is assigned
        if (player == null)
        {
            Debug.LogWarning("Player reference is not assigned.");
            return;
        }

        // Move towards the player's position
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Update animation parameters based on movement direction and speed
        UpdateAnimationParameters();
    }

    void UpdateAnimationParameters()
    {
        // Calculate movement direction
        Vector2 direction = (player.position - transform.position).normalized;

        // Flip the sprite based on the direction of movement
        if (direction.x < 0)
        {
            // Moving left, flip the sprite
            transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        }
        else if (direction.x > 0)
        {
            // Moving right, un-flip the sprite
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }

        // Update the Horizontal animation parameter based on movement direction
        animator.SetFloat("Horizontal", Mathf.Abs(direction.x));

        // Optionally, you can update the speed parameter if you have it
        animator.SetFloat("Speed", direction.magnitude * moveSpeed);
    }
}
