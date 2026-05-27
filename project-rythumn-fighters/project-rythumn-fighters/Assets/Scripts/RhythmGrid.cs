using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Events;
using System.Collections.Generic;
 


public class RhythmGrid : MonoBehaviour
{
    [Header("Grid Layout")]
    public int cellSize = 60;
    public int labelWidth = 40;
    public int headerHeight = 40;
    public Vector2 gridOffset = new Vector2(100, 100);

    [Header("Events")]
    public UnityEvent onAllDotsCompleted;

    [Header("Timing")]
    [Tooltip("Beats per minute — controls how fast the scan line moves across the 4-column grid")]
    public float bpm = 120f;

    private float CycleDuration => 240f / Mathf.Max(bpm, 1f);

    [Header("Highlight")]
    [Tooltip("How long a hit cell stays highlighted (seconds)")]
    public float highlightDuration = 0.4f;

    // 4 rows (A-D) x 4 columns (1-4)
   // private bool[,] dots = new bool[4, 4];
    private HashSet<System.Drawing.Point> dots = new HashSet<System.Drawing.Point>();
    private HashSet<System.Drawing.Point> completed = new HashSet<System.Drawing.Point>();
    private float[,] highlightTimer = new float[4, 4];
    private float lineProgress;

    private static readonly Key[] RowKeys = { Key.A, Key.S, Key.D, Key.F };
    private static readonly string[]  RowLabels = { "A", "S", "D", "F" };

    [Header("Visuals")]
    public Texture2D buttonTexture;

    [Header("Visuals")]
    public Texture2D pressedButtonTexture;

    [Header("Visuals")]
    public Texture2D hitButtonTexture;

    private Texture2D white;
    private GUIStyle labelStyle;

    void Awake()
    {
        white = new Texture2D(1, 1);
        white.SetPixel(0, 0, Color.white);
        white.Apply();

        // Default checkerboard-style dot pattern — edit these in code or expose via Inspector
        //dots[0, 0] = true; 
        //dots[1, 3] = true;
        //dots[3, 2] = true;
        dots.Add(new System.Drawing.Point(0, 0));
        dots.Add(new System.Drawing.Point(1, 3));
        dots.Add(new System.Drawing.Point(3, 2));
    }

    void Update()
    {
        lineProgress = (lineProgress + Time.deltaTime / CycleDuration) % 1f;

        for (int r = 0; r < 4; r++)
            for (int c = 0; c < 4; c++)
                if (highlightTimer[r, c] > 0f)
                    highlightTimer[r, c] -= Time.deltaTime;

        int lineCol = Mathf.Clamp(Mathf.FloorToInt(lineProgress * 4), 0, 3);
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            for (int r = 0; r < 4; r++)
            {
                var thisPoint = new System.Drawing.Point(r, lineCol);
                if (keyboard[RowKeys[r]].wasPressedThisFrame && dots.Contains(thisPoint))
                {
                    highlightTimer[r, lineCol] = highlightDuration;
                    completed.Add(new System.Drawing.Point(r, lineCol));
                    //completd[r, lineCol] = true;
                }
            }        
                    
        }
    }

    void OnGUI()
    {
        if (labelStyle == null)
        {
            labelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize  = 20,
                fontStyle = FontStyle.Bold
            };
            labelStyle.normal.textColor = Color.white;
        }

        int ox = (int)gridOffset.x;
        int oy = (int)gridOffset.y;

        // Column number headers (1-4)
        for (int c = 0; c < 4; c++)
        {
            Rect r = new Rect(ox + labelWidth + c * cellSize, oy, cellSize, headerHeight);
            Fill(r, new Color(0.18f, 0.18f, 0.28f));
            Outline(r, new Color(0.5f, 0.5f, 0.65f));
            GUI.Label(r, (c + 1).ToString(), labelStyle);
        }
        bool allCompleted = true;
        // Grid rows A-D
        for (int row = 0; row < 4; row++)
        {
            int ry = oy + headerHeight + row * cellSize;

            // Row letter label
            Rect lbl = new Rect(ox, ry, labelWidth, cellSize);
            Fill(lbl, new Color(0.18f, 0.18f, 0.28f));
            Outline(lbl, new Color(0.5f, 0.5f, 0.65f));
            GUI.Label(lbl, RowLabels[row], labelStyle);
    
            // Play cells
            for (int col = 0; col < 4; col++)
            {
                Rect cell = new Rect(ox + labelWidth + col * cellSize, ry, cellSize, cellSize);
                bool hit  = highlightTimer[row, col] > 0f;

                Fill(cell, hit ? new Color(1f, 0.85f, 0.1f) : new Color(0.11f, 0.11f, 0.17f));
                Outline(cell, new Color(0.35f, 0.35f, 0.48f));
                var p = new System.Drawing.Point(row, col);

                if (dots.Contains(p))
                {
                    float pad = cellSize * 0.28f;
                    Rect dot = new Rect(cell.x + pad, cell.y + pad, cell.width - pad * 2f, cell.height - pad * 2f);

                    bool isCompleted = completed.Contains(p);
                    if(!isCompleted)
                        allCompleted = false;
                    Texture2D b = buttonTexture;
                    if (hit)
                        b = hitButtonTexture;
                    if (isCompleted)
                        b = pressedButtonTexture;
                    GUI.DrawTexture(dot, b, ScaleMode.ScaleToFit);
                    
                }
            }
        }

        // Scanning line (red, 3px wide)
        float lx = ox + labelWidth + lineProgress * 4 * cellSize;
        Fill(new Rect(lx - 1.5f, oy, 3f, headerHeight + 4 * cellSize), new Color(1f, 0.2f, 0.2f, 0.85f));
    
        if(allCompleted)
        {
            onAllDotsCompleted?.Invoke();

            // reset dots and completed
            // and set a random number of dots 1 - 4
            // with only one dot per column
            dots.Clear();
            completed.Clear();
            int numDots = Random.Range(1, 5);
            List<int> availableCols = new List<int> { 0, 1, 2, 3 };
            for (int i = 0; i < numDots; i++)
            {
                int colIndex = Random.Range(0, availableCols.Count);
                int col = availableCols[colIndex];
                availableCols.RemoveAt(colIndex);
                int row = Random.Range(0, 4);
                dots.Add(new System.Drawing.Point(row, col));
            }
        }
    }

    // --- Drawing helpers ---

    void Fill(Rect r, Color c)
    {
        Color prev = GUI.color;
        GUI.color = c;
        GUI.DrawTexture(r, white);
        GUI.color = prev;
    }

    void Outline(Rect r, Color c)
    {
        Fill(new Rect(r.x,        r.y,        r.width, 1),       c);
        Fill(new Rect(r.x,        r.yMax - 1, r.width, 1),       c);
        Fill(new Rect(r.x,        r.y,        1,       r.height), c);
        Fill(new Rect(r.xMax - 1, r.y,        1,       r.height), c);
    }

    // --- Public API for setting dots at runtime ---

    //public void SetDot(int row, int col, bool value)
    //{
    //    if (row >= 0 && row < 4 && col >= 0 && col < 4)
    //        dots[row, col] = value;
    //}

    //public bool GetDot(int row, int col) =>
    //    row >= 0 && row < 4 && col >= 0 && col < 4 && dots[row, col];
}
