using UnityEngine;

public class SoundEffectPayer : MonoBehaviour
{
   public AudioSource src;
    public AudioClip sfx1, sfx2, sfx3;

    public void Level1()
    {
        src.clip = sfx1;
        src.Play();
    }
    public void Level2()
    {
        src.clip = sfx2;
        src.Play();
    }
    public void Level3()
    {
        src.clip = sfx3;
        src.Play();
    }
}
