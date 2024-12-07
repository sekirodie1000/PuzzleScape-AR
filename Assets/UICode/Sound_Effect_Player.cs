using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1, sfx2, sfx3;

    public void Exit()
    {
        src.clip = sfx1;
        src.Play();
    }
    public void Replay()
    {
        src.clip = sfx2;
        src.Play();
    }
    public void Next()
    {
        src.clip = sfx3;
        src.Play();
    }
}
