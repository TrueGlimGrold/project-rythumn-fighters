using System.Collections.Generic;
using UnityEngine;

public class SimpleComposer : MonoBehaviour
{

    private int currentBar = 1;
    private int beatCount = 0;

    [System.Serializable]
    public class BeatNote
    {
        public int beat;
        public bool buttonA;
        public bool buttonB;
        public bool buttonC;
    }

    public List<BeatNote> beatNotes = new List<BeatNote>();

    public static event System.Action<BeatNote> OnBeatNote;

    void OnEnable() => Metronome.OnBeat += HandleBeat;
    void OnDisable() => Metronome.OnBeat -= HandleBeat;

    void HandleBeat(int beatNumber)
    {
        BeatNote note = beatNotes.Find(n => n.beat == beatNumber);
        OnBeatNote?.Invoke(note);
    }
}
