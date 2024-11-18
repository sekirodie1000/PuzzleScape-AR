using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCleared : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadSceneAsync("Level selection");
    }
        
}
