using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSelectionButtons : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadSceneAsync("Story1");
    }
    public void Level2()
    {
        SceneManager.LoadSceneAsync("Tool description");
    }
    public void Level3()
    {
        SceneManager.LoadSceneAsync("Tool description2");
    }
}
