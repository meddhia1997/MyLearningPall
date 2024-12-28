using UnityEngine;

public class AnimationControllerAssigner : MonoBehaviour
{
    public Animator playerAnimator;  // Reference to the player's Animator component
    public RuntimeAnimatorController[] animatorControllers;  // Array of animation controllers
    public string[] controllerNames;  // Array of names corresponding to the animation controllers

    void Start()
    {
        // Check if playerAnimator is not null
        if (playerAnimator != null)
        {
            // Get the skinName from PlayerPrefs
            string skinName = PlayerPrefs.GetString("SkinName");

            // Find the index of the skinName in controllerNames array
            int index = System.Array.IndexOf(controllerNames, skinName);

            // Check if the index is valid and animatorControllers array is not null
            if (index != -1 && animatorControllers != null && index < animatorControllers.Length)
            {
                // Assign the corresponding animatorController to the playerAnimator
                playerAnimator.runtimeAnimatorController = animatorControllers[index];
                Debug.Log("Animation controller assigned successfully.");
            }
            else
            {
                Debug.LogError("Invalid skinName or Animator Controller array is not assigned properly.");
            }
        }
        else
        {
            Debug.LogError("Player Animator is not assigned.");
        }
    }
}
