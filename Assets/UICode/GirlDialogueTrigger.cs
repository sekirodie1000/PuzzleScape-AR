using UnityEngine;

public class GirlDialogueTrigger : MonoBehaviour
{
    public GameObject dialogueButton;
    private bool hasTriggered = false;

    public void ShowDialogue()
    {
        if (hasTriggered) return;

        hasTriggered = true;

        if (dialogueButton != null)
        {
            dialogueButton.SetActive(true);
        }
    }
}
