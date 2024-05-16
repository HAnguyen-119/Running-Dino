using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;
    public AudioSource AudioSource { get => audioSource; } 
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip jumpSound;
    public AudioClip deathSound;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMenuMusic()
    {
        audioSource.clip = menuMusic;
        audioSource.PlayDelayed(1);
    }

    public void PlayGameMusic()
    {
        audioSource.clip = gameplayMusic;
        audioSource.PlayDelayed(1);
    }

    public void PlayJumpSoundEffect()
    {
        audioSource.PlayOneShot(jumpSound, 0.1f);
    }

    public void PlayDeathSoundEffect()
    {
        audioSource.PlayOneShot(deathSound, 0.25f);
    }
}
