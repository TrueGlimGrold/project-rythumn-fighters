using UnityEngine;

public class ButtonJudge : MonoBehaviour
{
    
    public AudioClip buttonASound;
    public AudioClip buttonBSound;
    public AudioClip buttonCSound;
    public AudioClip buttonDSound;
    public AudioClip wrongSound;

    private AudioSource _audioSource;
    private string _correctButton = "";

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SimpleComposer.OnCorrectButton += HandleCorrectButton;
        PlayerInputHandler.OnA_ButtonPressed += () => CheckButton("A");
        PlayerInputHandler.OnB_ButtonPressed += () => CheckButton("B");
        PlayerInputHandler.OnC_ButtonPressed += () => CheckButton("C");
        PlayerInputHandler.OnD_ButtonPressed += () => CheckButton("D");
    }

    void OnDisable()
    {
        SimpleComposer.OnCorrectButton -= HandleCorrectButton;
        PlayerInputHandler.OnA_ButtonPressed -= () => CheckButton("A");
        PlayerInputHandler.OnB_ButtonPressed -= () => CheckButton("B");
        PlayerInputHandler.OnC_ButtonPressed -= () => CheckButton("C");
        PlayerInputHandler.OnD_ButtonPressed -= () => CheckButton("D");
    }

    void HandleCorrectButton(string button)
    {
        _correctButton = button;
    }

    void CheckButton(string pressedButton)
    {
        if (pressedButton == _correctButton)
        {
            PlayButtonSound(pressedButton);
            Debug.Log($"Correct {pressedButton}");
        }
        else
        {
            _audioSource.PlayOneShot(wrongSound);
            Debug.Log($"WrongPressed!!! {pressedButton}, correct was {_correctButton}");
        }
    }

    void PlayButtonSound(string button)
    {
        AudioClip clip = button switch
        {
            "A" => buttonASound,
            "B" => buttonBSound,
            "C" => buttonCSound,
            "D" => buttonDSound,
            _ => null
        };

        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }
}