using NUnit.Framework;
using UnityEngine;

public class ButtonJudge : MonoBehaviour
{
    
    public AudioClip buttonASound;
    public AudioClip buttonBSound;
    public AudioClip buttonCSound;
    public AudioClip buttonDSound;
    public AudioClip wrongSound;

    private AudioSource audioSource;
    private SimpleComposer.BeatNote BeatNote;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SimpleComposer.OnBeatNote += HandleBeatNote;
        PlayerInputHandler.OnA_ButtonPressed += () => CheckButton("A");
        PlayerInputHandler.OnB_ButtonPressed += () => CheckButton("B");
        PlayerInputHandler.OnC_ButtonPressed += () => CheckButton("C");
        PlayerInputHandler.OnD_ButtonPressed += () => CheckButton("D");
    }

    private void OnDisable()
    {
        SimpleComposer.OnBeatNote -= HandleBeatNote;
        PlayerInputHandler.OnA_ButtonPressed -= () => CheckButton("A");
        PlayerInputHandler.OnB_ButtonPressed -= () => CheckButton("B");
        PlayerInputHandler.OnC_ButtonPressed -= () => CheckButton("C");
        PlayerInputHandler.OnD_ButtonPressed -= () => CheckButton("D");
    }

    private void HandleBeatNote(SimpleComposer.BeatNote note)
    {
        BeatNote = note;
    }

    private void CheckButton(string pressed)
    {
        if (BeatNote == null) { audioSource.PlayOneShot(wrongSound); return; }

        bool correct = pressed switch
        {
            "A" => BeatNote.buttonA,
            "B" => BeatNote.buttonB,
            "C" => BeatNote.buttonC,
            _ => false
        };

        if (correct)
        {
            AudioClip clip = pressed switch
            {
                "A" => buttonASound,
                "B" => buttonBSound,
                "C" => buttonCSound,
                _ => null
            };
            if (clip) audioSource.PlayOneShot(clip);
            Debug.Log($"Correct! {pressed}");
        }
        else
        {
            audioSource.PlayOneShot(wrongSound);
            Debug.Log($"WrongPressed!!! {pressed}, correct was {correct}");
        }
    }

   
}