using UnityEngine;

public class SoundEffectMainMenu : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1, sfx2, sfx3;

    public void Start()
    {
        src.clip = sfx1;
        src.Play();
    }
    public void Settings()
    {
        src.clip = sfx2;
        src.Play();
    }
    public void Quit()
    {
        src.clip = sfx3;
        src.Play();
    }
}
