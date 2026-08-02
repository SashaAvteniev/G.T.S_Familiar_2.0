using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.XR;

public class PianoPuzzleManager : MonoBehaviour
{
    public List<int> puzzleNoteQueue;
    [SerializeField] List<int> firstBar;
    [SerializeField] List<int> secondBar;
    [SerializeField] List<int> thirdBar;

    [SerializeField] GameObject barOneObject;
    [SerializeField] GameObject barTwoObject;
    [SerializeField] GameObject barThreeObject;
    
    private MeshRenderer barOneMeshRenderer;
    private MeshRenderer barTwoMeshRenderer;
    private MeshRenderer barThreeMeshRenderer;

    [SerializeField]
    [HideInInspector]
    private Material mSuccess;
    
    [SerializeField]
    [HideInInspector]
    private Material mFailure;

    [SerializeField]
    [HideInInspector]
    private GameObject talisman; 
    
    // Default value is false
    private bool correctOrderFirst;
    private bool correctOrderSecond;
    private bool correctOrderThird;
    
    private AudioSource pianoAudio;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async void Start()
    {
        pianoAudio = GetComponent<AudioSource>();
        mSuccess = await Addressables.LoadAssetAsync<Material>("BarSuccess").Task;
        mFailure = await Addressables.LoadAssetAsync<Material>("BarFail").Task;
        
        // GetComponent is an expensive function, so we want to call it once per object and store the result
        barOneMeshRenderer = barOneObject.GetComponent<MeshRenderer>();
        barTwoMeshRenderer = barTwoObject.GetComponent<MeshRenderer>();
        barThreeMeshRenderer = barThreeObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleNoteQueue.Count == 4 && !correctOrderFirst)
        {
            correctOrderFirst = puzzleNoteQueue.SequenceEqual(firstBar);
            if (correctOrderFirst)
            {
                puzzleNoteQueue.Clear();
                barOneMeshRenderer.material = mSuccess;
            }
            else
            {
                puzzleNoteQueue.Clear();
                barOneMeshRenderer.material = mFailure;
            }
        }
        else if(puzzleNoteQueue.Count == 4 && correctOrderFirst && !correctOrderSecond)
        {
            correctOrderSecond = puzzleNoteQueue.SequenceEqual(secondBar);
            if (correctOrderSecond)
            {
                puzzleNoteQueue.Clear();
                barTwoMeshRenderer.material = mSuccess;
            }
            else
            {
                puzzleNoteQueue.Clear();
                barOneMeshRenderer.material = mFailure;
                barTwoMeshRenderer.material = mFailure;
                correctOrderFirst = false;
            }
        }
        if (puzzleNoteQueue.Count == 4 && correctOrderFirst && correctOrderSecond)
        {
            correctOrderThird = puzzleNoteQueue.SequenceEqual(thirdBar);
            if (correctOrderThird)
            {
                Debug.Log("thirdBarDone");
                barThreeMeshRenderer.material = mSuccess;
                puzzleNoteQueue.Clear();
                
                // Spawn talisman, will be 1 unit above player
                Transform spawnPos = GameManager.player.transform;
                spawnPos.position = new Vector3(GameManager.player.transform.position.x,
                    GameManager.player.transform.position.y + 1,
                    GameManager.player.transform.position.z);
                Addressables.InstantiateAsync(Addressables.LoadAssetAsync<GameObject>("Talisman").Result, spawnPos);
            }
            else
            {
                puzzleNoteQueue.Clear();
                correctOrderFirst = false;
                correctOrderSecond = false;
                barOneMeshRenderer.material = mFailure;
                barTwoMeshRenderer.material = mFailure;
                barThreeMeshRenderer.material = mFailure;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PuzzleObjectPushScript puzzleObject))
        {
            puzzleNoteQueue.Add(puzzleObject.NoteValue);
            pianoAudio.generator = puzzleObject.SoundQueue;
            pianoAudio.Play();
            puzzleObject.Reset();
        }
    }
}
