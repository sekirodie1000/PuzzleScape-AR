using UnityEngine;
using TMPro;

public class DialogueButtonController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] dialogues;
    private int currentDialogueIndex = 0;

    void Start()
    {
        if (dialogues.Length > 0)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
        }
        else
        {
            Debug.LogWarning("No dialogues set in the array!");
        }
    }

    public void OnDialogueClick()
    {
        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentDialogueIndex];
        }
        else
        {
            dialogueText.text = "";
            gameObject.SetActive(false);
        }
    }
}
