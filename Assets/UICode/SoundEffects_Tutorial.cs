using UnityEngine;

public class SoundEffects_Tutorial : MonoBehaviour
{
    
    public AudioSource src;
    public AudioClip sfx1;

    public void Skip()
    {
        src.clip = sfx1;
        src.Play();
    }

}
