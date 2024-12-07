using System.Collections;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class TextWriter : MonoBehaviour
{
    private TMP_Text uiText; // Use TMP_Text for TextMeshPro
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private float timer;

    public void AddWriter(TMP_Text uiText, string textToWrite, float timePerCharacter)
    {
        this.uiText = uiText; // Assign TMP_Text component
        this.textToWrite = textToWrite; // Text to display
        this.timePerCharacter = timePerCharacter; // Speed of writing
        characterIndex = 0; // Reset character index
        timer = 0f; // Reset timer
        uiText.text = ""; // Clear text initially
    }

    private void Update()
    {
        if (uiText != null && characterIndex < textToWrite.Length)
        {
            timer -= Time.deltaTime;
            while (timer <= 0f)
            {
                // Display next character
                timer += timePerCharacter;
                characterIndex++;
                uiText.text = textToWrite.Substring(0, characterIndex);

                if (characterIndex >= textToWrite.Length) {
                    // Entire string displayed
                    uiText = null;
                    return;
                }
            }
        }
    }
}

