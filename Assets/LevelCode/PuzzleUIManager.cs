using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PuzzleUIManager : MonoBehaviour
{
    public GameObject popupPanel;
    public TMP_Text popupText;
    public Button beginButton;
    public Button nextSceneButton;

    void Start()
    {
        if (popupPanel != null) popupPanel.SetActive(false);
        if (beginButton != null) beginButton.gameObject.SetActive(false);
        if (nextSceneButton != null) nextSceneButton.gameObject.SetActive(false);
    }

    public void ShowPopup(string msg, bool showBegin = false, string nextScene = null, int fontSize = 36, Color? fontColor = null, TMP_FontAsset customFont = null)
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
            popupText.text = msg;

            popupText.fontSize = fontSize;
            popupText.color = fontColor ?? Color.white;

            if (customFont != null)
            {
                popupText.font = customFont;
            }

            beginButton.gameObject.SetActive(showBegin);
            nextSceneButton.gameObject.SetActive(!string.IsNullOrEmpty(nextScene));

            if (!string.IsNullOrEmpty(nextScene))
            {
                nextSceneButton.onClick.RemoveAllListeners();
                nextSceneButton.onClick.AddListener(() => SceneManager.LoadScene(nextScene));
            }
        }
    }


    public void OnBeginClicked()
    {
        popupPanel.SetActive(false);
        FindObjectOfType<WireGenerator1>().SetPuzzleModeStarted(true);
    }

    public void OnClosePopupClicked()
    {
        popupPanel.SetActive(false);
    }
}


