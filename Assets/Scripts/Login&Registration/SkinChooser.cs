using UnityEngine;
using UnityEngine.UI;

public class SkinChooser : MonoBehaviour
{
    public RuntimeAnimatorController[] animationControllers; // Array to hold AnimationControllers
    public string[] skinNames;                               // Array to hold names corresponding to each AnimationController
    public Button nextButton;                                // Button to move to the next animation
    public Button backButton;                                // Button to move to the previous animation
    public Animator animator;                                // Reference to the Animator component where animations will be played

    public int currentIndex = 0;                            // Index of the current animation controller being displayed

    void Start()
    {
        // Set up button click events
        nextButton.onClick.AddListener(NextAnimation);
        backButton.onClick.AddListener(PreviousAnimation);

        // Display the initial animation
        ShowAnimation(currentIndex);
    }

    void NextAnimation()
    {
        currentIndex++;
        if (currentIndex >= animationControllers.Length)
        {
            currentIndex = 0; // Loop back to the first animation if at the end
        }
        ShowAnimation(currentIndex);
    }

    void PreviousAnimation()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = animationControllers.Length - 1; // Loop back to the last animation if at the beginning
        }
        ShowAnimation(currentIndex);
    }

    void ShowAnimation(int index)
    {
        // Set the animator's controller to the animation controller from the array at the specified index
        animator.runtimeAnimatorController = animationControllers[index];

        // Debug the selected skin name
        Debug.Log("Selected skin: " + skinNames[index]);
    }
}
