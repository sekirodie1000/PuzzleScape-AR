using UnityEngine;
using UnityEngine.SceneManagement;

public class Play : MonoBehaviour
{
    public void Tutorial1()
    {
        SceneManager.LoadSceneAsync("SeriesTutorial");
    }
    public void Tutorial2()
    {
        SceneManager.LoadSceneAsync("ParallelTutorial");
    }
}
