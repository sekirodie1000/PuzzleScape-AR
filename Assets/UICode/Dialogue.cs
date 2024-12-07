using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    public string nextSceneName;

    // Events to notify when dialogue starts or ends
    public event Action OnDialogueStart;
    public event Action OnDialogueEnd;

    void Start()
    {
        textSpeed = Mathf.Clamp(textSpeed, 0.01f, 0.5f); // Ensure reasonable range
        Debug.Log($"Final textSpeed: {textSpeed}"); // Log the clamped value
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        Debug.Log($"Frame Rate: {1 / Time.deltaTime}");
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        // Trigger the OnDialogueStart event
        Debug.Log("Dialogue started");
        OnDialogueStart?.Invoke();

        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        float startTime = Time.realtimeSinceStartup;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            while (Time.realtimeSinceStartup < startTime + textSpeed)
            {
                yield return null; // Wait until the real-time delay has passed
            }
            startTime += textSpeed;
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            // Trigger the OnDialogueEnd event
            Debug.Log("Dialogue ended");
            OnDialogueEnd?.Invoke();

            // gameObject.SetActive(false);
            // Wait for a click to load the next scene
             StartCoroutine(WaitForNextScene());
        }
    }
    IEnumerator WaitForNextScene()
    {
        Debug.Log("Waiting for player to click to load next scene...");
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        LoadNextScene();
    }
    void LoadNextScene()
    {
        Debug.Log("Loading next scene...");
        SceneManager.LoadScene(nextSceneName); // Replace "SceneName" with your target scene name
    }

}
