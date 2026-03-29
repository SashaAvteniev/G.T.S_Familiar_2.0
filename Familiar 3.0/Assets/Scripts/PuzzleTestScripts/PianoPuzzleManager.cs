using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PianoPuzzleManager : MonoBehaviour
{
    public List<int> puzzleNoteQueue;
    [SerializeField] List<int> correctPuzzleOrder;
    bool correctOrder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleNoteQueue.Count == 12)
        {
            correctOrder = puzzleNoteQueue.SequenceEqual(correctPuzzleOrder);
        }
        
        
        if (correctOrder)
        {
            Debug.Log("YIPPEEEEE");
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        puzzleNoteQueue.Add(collision.gameObject.GetComponent<PuzzleObjectPushScript>().NoteValue);
        collision.gameObject.GetComponent<PuzzleObjectPushScript>().Reset();
    }
}
