using UnityEngine;
using UnityEngine.SceneManagement;

public class Next : MonoBehaviour
{
    public void NextLevel1()
    {
        SceneManager.LoadSceneAsync("Tool description");
    }
    public void NextLevel2()
    {
        SceneManager.LoadSceneAsync("Tool description2");
    }
    public void NextLevel3()
    {
        SceneManager.LoadSceneAsync("Story4");
    }
    public void FinishSeries()
    {
        SceneManager.LoadSceneAsync("Tutorial 2");
    }
    public void FinishParallel()
    {
        SceneManager.LoadSceneAsync("Story3");
    }

    public void FinishLevel2()
    {
        SceneManager.LoadSceneAsync("Level2 Cleared");
    }
    public void FinishLevel3()
    {
        SceneManager.LoadSceneAsync("Level3 Cleared");
    }
    public void RepeatLevel1()
    {
        SceneManager.LoadSceneAsync("Tutorial 1");
    }
    public void RepeatLevel2()
    {
        SceneManager.LoadSceneAsync("Level2 description");
    }
    public void RepeatLevel3()
    {
        SceneManager.LoadSceneAsync("Level3 description");
    }

}
