using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] private float bpm = 120f; 
    [SerializeField] private float margin = 100f;

    private float beatDurationMs;
    private int lastBeat = 0;
    private float nextbeatPosition;

    private int activeBeat = -1;
    private float activeBeatStartPosition;
    private float activeBeatEndPosition;


    public static event System.Action<int> OnBeat;

    private void Start()
    {
        beatDurationMs = 60 / bpm * 1000; 
        nextbeatPosition = beatDurationMs;
    }

    private void Update()
    {
        float postion = SoundManager.Instance.TimePositionMs;

        if (postion >= nextbeatPosition)
        {
            lastBeat = (lastBeat % 4) + 1;
            OnBeat?.Invoke(lastBeat);

                activeBeat = lastBeat;
                activeBeatStartPosition = nextbeatPosition - margin;
                activeBeatEndPosition = nextbeatPosition + margin;

            nextbeatPosition += beatDurationMs;

            //Debug.Log($"beat {lastBeat}");
        }
    }

}
