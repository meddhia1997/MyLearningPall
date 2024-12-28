using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    
    private Vector2 movement;
    private Transform Player;
    public Animator animator;
    private PlayerInput playerInput;
    void Start()
    {
        // Get the PlayerInput component attached to the GameObject
        playerInput = GetComponent<PlayerInput>();

    }

    void Update()
    {
        // Read the value of the "Move" action from PlayerInput
        Vector2 inputs = playerInput.actions["Move"].ReadValue<Vector2>();

        // Assign the inputs directly to the movement vector
        movement = inputs;

        // Update animation based on the movement input
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Convert Player's position to string and assign it to the place text
    }

    void FixedUpdate()
    {
        // Apply movement to the player's rigidbody in the FixedUpdate method
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
