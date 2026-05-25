using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioClip song;

    public float TimePositionMs { get; private set; }

    private void Awake()
    {
       if (Instance != null && Instance != this)
       {
           Destroy(gameObject);
           return;
        }
       
         Instance = this;
    }

    private void Start()
    {
        musicSource.clip = song;
        musicSource.Play();
    }

    private void Update()
    {
        if (musicSource.isPlaying)
        {
            TimePositionMs = (float)musicSource.timeSamples / musicSource.clip.frequency * 1000f;
        }
    }
}
