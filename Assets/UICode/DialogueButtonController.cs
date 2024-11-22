using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueButtonController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Reference to the text component
    public string[] dialogues;          // Array of dialogue strings
    private int currentDialogueIndex = 0; // Keeps track of the current dialogue index
    public float typingSpeed = 0.05f;    // Speed of the typing effect
    public TMP_FontAsset pixelFont; // Drag the font here in the Inspector


    private Coroutine typingCoroutine;  // Reference to the current typing coroutine

    void Start()
    {
        if (pixelFont != null)
        {
            dialogueText.font = pixelFont; // Apply the font
        }
        else
        {
            Debug.LogWarning("Pixel font not assigned in the Inspector!");
        }
        dialogueText.fontSize = 40; // Set the font size to 50
        if (dialogues.Length > 0)
        {
            StartTypingDialogue(dialogues[currentDialogueIndex]);
        }
        else
        {
            Debug.LogWarning("No dialogues set in the array!");
        }
    }

    public void OnDialogueClick()
    {
        if (typingCoroutine != null)
        {
            // If the text is still typing, skip to the full dialogue
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogues[currentDialogueIndex];
            typingCoroutine = null;
            return;
        }

        currentDialogueIndex++;

        if (currentDialogueIndex < dialogues.Length)
        {
            StartTypingDialogue(dialogues[currentDialogueIndex]);
        }
        else
        {
            dialogueText.text = "";
            gameObject.SetActive(false); // Hide the button
        }
    }

    private void StartTypingDialogue(string dialogue)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(dialogue));
    }

    private IEnumerator TypeText(string dialogue)
    {
        dialogueText.text = ""; // Clear the text first

        foreach (char letter in dialogue.ToCharArray())
        {
            // Example of applying rich text dynamically
            /*if (char.IsUpper(letter))
            {
                dialogueText.text += $"<color=#FF0000><b>{letter}</b></color>"; // Red and bold for uppercase letters
            }
            else
            {
                dialogueText.text += $"<color=#00FF00>{letter}</color>"; // Green for lowercase letters
            }*/
            dialogueText.text += $"<color=#800080><b>{letter}</b></color>";


            yield return new WaitForSeconds(typingSpeed); // Wait for the next character
        }

        typingCoroutine = null; // Reset the coroutine reference
    }

}

