using UnityEngine;

public class DialogueSoundEffect : MonoBehaviour
{
    public AudioSource src;  // Assign the AudioSource in the Inspector.
    public AudioClip sfx4;   // Assign the audio clip in the Inspector.
    private bool isPlaying = false; // Prevent overlapping playback.

    // Corrected method with a generic float parameter.
    public void PlayAudioForSeconds(float duration)
    {
        if (!src.isPlaying)
        {
            src.clip = sfx4;
            src.Play();
            isPlaying = true;
            Invoke("StopAudio", duration); // Stop after the given duration.
        }
    }

    private void StopAudio()
    {
        src.Stop();
        isPlaying = false; // Reset the flag to allow re-triggering.
    }
}

