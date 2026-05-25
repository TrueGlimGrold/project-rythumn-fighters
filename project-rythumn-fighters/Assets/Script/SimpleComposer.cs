using System.Collections.Generic;
using UnityEngine;

public class SimpleComposer : MonoBehaviour
{
    [System.Serializable]
    public class BeatNote
    {
        public int beat;
        public string button; 
    }

    public List<BeatNote> beatNotes = new List<BeatNote>();

    public static event System.Action<string> OnCorrectButton;

    void OnEnable() => Metronome.OnBeat += HandleBeat;
    void OnDisable() => Metronome.OnBeat -= HandleBeat;

    void HandleBeat(int beatNumber)
    {
        BeatNote note = beatNotes.Find(n => n.beat == beatNumber);
        if (note != null)
            OnCorrectButton?.Invoke(note.button);
    }


}
