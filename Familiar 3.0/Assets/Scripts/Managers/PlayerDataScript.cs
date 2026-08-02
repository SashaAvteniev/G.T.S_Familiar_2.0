using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PlayerDataScript", menuName = "Scriptable Objects/PlayerDataScript")]
public class GameData : ScriptableObject
{
    [HideInInspector]
    [SerializeField]
    public PlayerData data;

    [HideInInspector]
    [SerializeField]
    public bool interactFlag = false;
    
    [SerializedDictionary("GUID", "Spawn Point")]
    
    public SerializedDictionary<string, Vector3> doorExits;

    [SerializeField] public string newDoorGUID;
    
    // Used in editor only and for before a save system
    void OnApplicationQuit()
    {
        interactFlag = false;
    }
}

[System.Serializable]
public struct PlayerData
{
    public enum ETalismans
    {
        None,
        Elk,
        Sheep,
        Snake,
        Badger
    }
    public ETalismans currentTalisman;
    public List<ETalismans> unlockedTalismans;
    public SceneInstance sceneInstance;

    public PlayerData(PlayerData playerData)
    {
        currentTalisman = playerData.currentTalisman;
        unlockedTalismans = playerData.unlockedTalismans;
        sceneInstance = playerData.sceneInstance;
    }
}