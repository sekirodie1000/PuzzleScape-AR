using UnityEngine;
using UnityEngine.SceneManagement;

public class skip : MonoBehaviour
{
    public void skipTutorial()
    {
        SceneManager.LoadSceneAsync("Level selection");
    }
}
