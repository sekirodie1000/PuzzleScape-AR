using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip background;

    // Singleton instance
    private static MusicManager instance;

    private void Awake()
    {
        // If an instance already exists and it's not this one, destroy the duplicate
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance to this object and prevent it from being destroyed
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Play music if not already playing
        if (!musicSource.isPlaying)
        {
            musicSource.clip = background;
            musicSource.loop = true; // Enable looping
            musicSource.Play();
        }
    }
}

