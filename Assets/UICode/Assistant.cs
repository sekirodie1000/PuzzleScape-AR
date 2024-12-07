using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class Assistant : MonoBehaviour
{
    [SerializeField] private TextWriter textWriter;
    private TMP_Text messageText; // Use TMP_Text for TextMeshPro

    private void Awake() {
        // Ensure you correctly find the "messageText" GameObject
        messageText = transform.Find("Message").Find("messageText").GetComponent<TMP_Text>();
        Application.targetFrameRate = 2;
    }

    private void Start() {
        //messageText.text = "Hello World!";
        textWriter.AddWriter(messageText, "Hello World!", 1f);
    }
}
