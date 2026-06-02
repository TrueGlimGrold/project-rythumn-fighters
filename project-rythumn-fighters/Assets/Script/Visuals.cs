using UnityEngine;

public class Visuals : MonoBehaviour
{
    [SerializeField] private int beatNum;
     SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
   {
       Metronome.OnBeat += (int beat) => HandleBeat(beat);
   }

   private void OnDisable()
   {
       Metronome.OnBeat -= HandleBeat;
   }

   private void HandleBeat(int beat)
   {
        if (beat == beatNum)
        {
           spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
