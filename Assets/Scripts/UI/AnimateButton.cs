using UnityEngine;
using UnityEngine.UI;

public class AnimateButton : MonoBehaviour
{
    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    // Reference to the Image component
    public Image image;

    void Start()
    {
        // Get the SpriteRenderer component attached to the same GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Check if the Image component is assigned
        if (image == null)
        {
            Debug.LogError("Image component is not assigned. Please assign an Image component to the script in the Inspector.");
            return;
        }

        // Update the image with the initial sprite
        UpdateImage();
    }

    void Update()
    {
        // Check if the sprite has changed
        if (spriteRenderer.sprite != image.sprite)
        {
            // Update the image with the new sprite
            UpdateImage();
        }
    }

    // Update the Image component with the sprite from the SpriteRenderer
    void UpdateImage()
    {
        image.sprite = spriteRenderer.sprite;
    }
}
