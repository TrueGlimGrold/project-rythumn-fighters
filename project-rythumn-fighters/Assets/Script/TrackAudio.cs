using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TrackAudio : MonoBehaviour
{
    [SerializeField] private NotePattern pattern;
    [SerializeField] private AudioClip clip;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Metronome.OnMeasure += Play;
    }

    private void OnDisable()
    {
        Metronome.OnMeasure -= Play;
    }

    private void Play(int measure)
    {
        if (pattern.measure[measure] == true)
        {
            source.PlayOneShot(clip);
        }
    }
}