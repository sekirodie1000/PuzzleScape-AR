using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PageClickToNext : MonoBehaviour
{
    public string nextSceneName;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            // Debug.Log("Screen clicked!");

            if (IsPointerOverUIElement())
            {
                // Debug.Log("Clicked on UI element. Ignoring screen click.");
                return;
            }

            // Debug.Log("Clicked on non-UI area. Loading next scene.");
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set!");
        }
    }

    private bool IsPointerOverUIElement()
    {
        if (EventSystem.current == null)
        {
            Debug.LogWarning("EventSystem not found in the scene!");
            return false;
        }

        return EventSystem.current.IsPointerOverGameObject();
    }
}
