using UnityEngine;

[CreateAssetMenu(menuName = "Lane/NotePattern")]
public class NotePattern : ScriptableObject
{
    [Tooltip("4 measures on a bar")]
    public bool[] measure = new bool[8];
}