using UnityEngine;

public class MetronomeBallVisual : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private Transform[] beatMarkers;

    private float beatDurSeconds;
    private float beatStartTime;
    private int currentBeat;
    private bool started = false;

    private void OnEnable()
    {
        Metronome.OnBeat += HandleBeat;
    }

    private void OnDisable()
    {
        Metronome.OnBeat -= HandleBeat;
    }

    private void HandleBeat(int beat)
    {
        beatDurSeconds = 60f / Metronome.Instance.bpm;
        beatStartTime = Time.time;
        currentBeat = (beat - 1) % beatMarkers.Length;
        started = true;
    }

    private void Update()
    {
        if (!started || beatMarkers.Length < 2) return;

        float progress = (Time.time - beatStartTime) / beatDurSeconds;
        progress = Mathf.Clamp01(progress);

        int nextBeat = (currentBeat + 1) % beatMarkers.Length;

        ball.position = Vector3.Lerp(
            beatMarkers[currentBeat].position,
            beatMarkers[nextBeat].position,
            progress
        );
    }
}