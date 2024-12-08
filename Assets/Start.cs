using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public void Level2()
    {
        SceneManager.LoadSceneAsync("Level2");
    }
    public void Level3()
    {
        SceneManager.LoadSceneAsync("Level3");
    }
}
