using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PianoPuzzleManager : MonoBehaviour
{
    public List<int> puzzleNoteQueue;
    [SerializeField] List<int> firstBar;
    [SerializeField] List<int> secondBar;
    [SerializeField] List<int> thirdBar;
    bool correctOrderFirst;
    bool correctOrderSecond;
    bool correctOrderThird;

    [SerializeField] GameObject barOneObject;
    [SerializeField] GameObject barTwoObject;
    [SerializeField] GameObject barThreeObject;

    [SerializeField] Material success;
    [SerializeField] Material failure;

    [SerializeField] GameObject talisman; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        correctOrderFirst = false;
        correctOrderSecond = false;
        correctOrderThird = false;
        talisman.SetActive(false);
        talisman.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleNoteQueue.Count == 4 && !correctOrderFirst)
        {
            correctOrderFirst = puzzleNoteQueue.SequenceEqual(firstBar);
            if (correctOrderFirst)
            {
                Debug.Log("firstBarDone");
                puzzleNoteQueue.Clear();
                barOneObject.GetComponent<MeshRenderer>().material = success;
            }
            else
            {
                puzzleNoteQueue.Clear();
                barOneObject.GetComponent<MeshRenderer>().material = failure;
            }
        }
        else if(puzzleNoteQueue.Count == 4 && correctOrderFirst && !correctOrderSecond)
        {
            correctOrderSecond = puzzleNoteQueue.SequenceEqual(secondBar);
            if (correctOrderSecond)
            {
                Debug.Log("secondBarDone");
                puzzleNoteQueue.Clear();
                barTwoObject.GetComponent<MeshRenderer>().material = success;
            }
            else
            {
                puzzleNoteQueue.Clear();
                barOneObject.GetComponent<MeshRenderer>().material = failure;
                barTwoObject.GetComponent<MeshRenderer>().material = failure;
                correctOrderFirst = false;
            }
        }
        if (puzzleNoteQueue.Count == 4 && correctOrderFirst && correctOrderSecond)
        {
            correctOrderThird = puzzleNoteQueue.SequenceEqual(thirdBar);
            if (correctOrderThird)
            {
                Debug.Log("thirdBarDone");
                barThreeObject.GetComponent<MeshRenderer>().material = success;
                puzzleNoteQueue.Clear();
                talisman.SetActive(true);
                talisman.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                puzzleNoteQueue.Clear();
                correctOrderFirst = false;
                correctOrderSecond = false;
                barOneObject.GetComponent<MeshRenderer>().material = failure;
                barTwoObject.GetComponent<MeshRenderer>().material = failure;
                barThreeObject.GetComponent<MeshRenderer>().material = failure;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Shovable")
        {
            puzzleNoteQueue.Add(collision.gameObject.GetComponent<PuzzleObjectPushScript>().NoteValue);
            collision.gameObject.GetComponent<PuzzleObjectPushScript>().Reset();
        }

    }
}
