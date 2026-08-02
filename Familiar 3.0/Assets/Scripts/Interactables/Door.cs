using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class Door : InteractParent
{
    private enum EExitDirection
    {
        X,
        Z,
        minusX,
        minusZ,
    }
    
    [Tooltip("GUID of the other door")]
    [SerializeField]
    private string linkedGUID;

    private Vector3 playerSpawn;

    void Start()
    {
        if (!GameManager.gameData.doorExits.ContainsKey(ID))
        {
            Init();
        }
    }
    
    public void Init()
    {
        playerSpawn = transform.position;
        playerSpawn += transform.forward;
        playerSpawn.y += 1;
        GameManager.gameData.doorExits.TryAdd(ID, playerSpawn);
    }
    
    protected override void Interact()
    {
        Dictionary<string, Vector3> doorExits = GameManager.gameData.doorExits;
        GameManager.gameData.newDoorGUID = linkedGUID;
        GameManager.gameData.interactFlag = false;
        LoadingScreenManager.Instance.SwitchScene(linkedGUID.Split("_")[0]);
    }
}
