using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class Door : InteractParent
{
    [Tooltip("GUID of the other door")]
    [SerializeField]
    private string linkedGUID; // The GUID of the connecting door in the other level, door will not work unless set

    private Vector3 playerSpawn;

    void Start()
    {
        // Check to see if the Game Manager already has a location saved for where to spawn the player.
        // Otherwise, we need to add this door to the dictionary for future use
        if (!GameManager.gameData.doorExits.ContainsKey(ID))
        {
            Init();
        }
    }
    
    // Find where we should spawn the player and try and add it to the master dictionary
    public void Init()
    {
        playerSpawn = transform.position;
        playerSpawn += transform.forward;
        playerSpawn.y += 1;
        GameManager.gameData.doorExits.TryAdd(ID, playerSpawn);
    }
    
    protected override void Interact()
    {
        GameManager.gameData.newDoorGUID = linkedGUID;
        GameManager.gameData.interactFlag = false; // We need to reset this, or we won't be able to interact with anything after the scene loads
        LoadingScreenManager.Instance.SwitchScene(linkedGUID.Split("_")[0]); // The scene name is saved in the beginning of the GUID so we can just split it via an underscore
    }
}
