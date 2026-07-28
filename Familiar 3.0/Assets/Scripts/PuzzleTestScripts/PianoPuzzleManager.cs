using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PianoPuzzleManager : MonoBehaviour
{
    //holds player input
    public List<int> puzzleNoteQueue;
    
    //correct order for melody 1-3
    [SerializeField] List<int> firstBar;
    [SerializeField] List<int> secondBar;
    [SerializeField] List<int> thirdBar;
   
    //has melody been completed
    bool correctOrderFirst;
    bool correctOrderSecond;
    bool correctOrderThird;

    //visual if melody correct or not 
    [SerializeField] GameObject barOneObject;
    [SerializeField] GameObject barTwoObject;
    [SerializeField] GameObject barThreeObject;
    [SerializeField] Material success;
    [SerializeField] Material failure;
    
    //reward unlocked when puzzle solved
    [SerializeField] GameObject talisman; 
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //puzzle set up
        correctOrderFirst = false;
        correctOrderSecond = false;
        correctOrderThird = false;
        talisman.SetActive(false);
        talisman.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        //only check when player has entered full bar
        if (puzzleNoteQueue.Count != 4) { return; }
        
        //check melody 1 (first 4 notes)
        if (!correctOrderFirst)
        {
            correctOrderFirst = puzzleNoteQueue.SequenceEqual(firstBar);
            
            if (correctOrderFirst)
            {
                Debug.Log("firstBarDone"); 
                SetBarMaterial(barOneObject, success);
            }
            else
            {
                //wrong sequence
                SetBarMaterial(barOneObject, failure);
            }
            
            puzzleNoteQueue.Clear();
        }
        //Check melody 2 (next 4 notes after melody 1)
        else if(!correctOrderSecond)
        {
            correctOrderSecond = puzzleNoteQueue.SequenceEqual(secondBar);
            
            if (correctOrderSecond)
            {
                Debug.Log("secondBarDone");
                SetBarMaterial(barTwoObject, success);
            }
            else
            {
                //wrong sequence
                SetBarMaterial(barOneObject, failure);
                SetBarMaterial(barTwoObject, failure);
            }
            
            puzzleNoteQueue.Clear();
        }
        //Check melody 3 (next 4 notes after melody 1 & 2)
        else if (!correctOrderThird)
        {
            correctOrderThird = puzzleNoteQueue.SequenceEqual(thirdBar);
            
            if (correctOrderThird)
            {
                Debug.Log("thirdBarDone");
                SetBarMaterial(barThreeObject, success);
                
                //puzzle complete, show & enable talisman
                talisman.SetActive(true);
                talisman.GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                //wrong sequence - full reset
                correctOrderFirst = false;
                correctOrderSecond = false;
                SetBarMaterial(barOneObject, failure);
                SetBarMaterial(barTwoObject, failure);
                SetBarMaterial(barThreeObject, failure);
            }
            
            puzzleNoteQueue.Clear();
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
    
    /// <summary>
    /// changes material of game objects
    /// </summary>
    /// <param name="bar">which melody to edit</param>
    /// <param name="material">material to change to</param>
    void SetBarMaterial(GameObject bar, Material material)
    {
        bar.GetComponent<MeshRenderer>().material = material;
    }
}
