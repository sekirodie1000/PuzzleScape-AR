using UnityEngine;

public class SoundEffects_Tutorial2 : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1, sfx2;

    public void Skip()
    {
        src.clip = sfx1;
        src.Play();
    }
    public void Play()
    {
        src.clip = sfx2;
        src.Play();
    }
}
