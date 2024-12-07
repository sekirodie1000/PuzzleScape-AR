using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageHandler : MonoBehaviour
{
    public Image girl;              // Assign the "girl" image here
    public Dialogue DialogueBox;    // Reference to your DialogueBox script
    private Animator animator;      // Animator attached to the "girl" GameObject

    private void Start()
    {
        // Get the Animator component from the "girl" GameObject
        if (girl != null)
        {
            animator = girl.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator not found on girl GameObject");
            }
        }

        // Ensure the DialogueBox is assigned
        if (DialogueBox != null)
        {
            DialogueBox.OnDialogueStart += ShowImage;
            DialogueBox.OnDialogueEnd += HideImage;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (DialogueBox != null)
        {
            DialogueBox.OnDialogueStart -= ShowImage;
            DialogueBox.OnDialogueEnd -= HideImage;
        }
    }

    private void ShowImage()
    {
        if (animator != null)
        {
            Debug.Log("ShowImage called, triggering Animator");
            animator.SetBool("IsOpen", true); // Set IsOpen to true to trigger the appear animation
        }
        else
        {
            Debug.LogError("Animator not assigned");
        }
    }

    private void HideImage()
    {
        if (animator != null)
        {
            Debug.Log("HideImage called, triggering Animator");
            animator.SetBool("IsOpen", false); // Set IsOpen to false to trigger the disappear animation
        }
        else
        {
            Debug.LogError("Animator not assigned");
        }
    }
}

