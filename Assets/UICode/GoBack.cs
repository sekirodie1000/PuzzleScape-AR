using UnityEngine;
using UnityEngine.SceneManagement;
public class GoBack : MonoBehaviour
{
    public void GoBackFunction()
    {
        SceneManager.LoadScene("mainMenu");
    }

}
