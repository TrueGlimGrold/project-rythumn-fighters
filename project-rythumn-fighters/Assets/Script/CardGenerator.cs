using UnityEngine;
using System;

public class CardGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool[] line1 = GenerateLine();
        for (int i = 0; i < line1.Length; i++)
        {
            Debug.Log(line1[i]);
        }
        Debug.Log(string.Join(", ", line1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool[] GenerateLine()
    {
        System.Random randomCardNum = new System.Random();
        int currentValue;


        bool[] line = new bool[4]; 
        for (int i = 0; i < 4; i++)
        {
            currentValue = randomCardNum.Next(0,2);
            if (currentValue == 0)
            {
                line[i] = false;
            }
            else
            {
                line[i] = true;
            }
        }
        return line;
    }
}
