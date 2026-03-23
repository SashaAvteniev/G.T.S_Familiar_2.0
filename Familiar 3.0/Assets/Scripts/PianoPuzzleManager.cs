using System;
using UnityEngine;

public class PianoPuzzleManager : MonoBehaviour
{
    public int[] puzzleNoteQueue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        puzzleNoteQueue = new int[12];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
