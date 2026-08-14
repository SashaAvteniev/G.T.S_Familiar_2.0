using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerDataScript", menuName = "Scriptable Objects/PlayerDataScript")]
public class GameData : ScriptableObject
{
    [FormerlySerializedAs("data")]
    [HideInInspector]
    [SerializeField]
    public PlayerData playerData; // The player specific data

    [HideInInspector]
    [SerializeField]
    public bool interactFlag = false; // Only here for future save system
    
    [SerializedDictionary("GUID", "Spawn Point")]
    [HideInInspector] //Remove to aid in debugging
    public SerializedDictionary<string, Vector3> doorExits;

    [HideInInspector]
    [SerializeField]
    public string newDoorGUID; // Which door GUID is the target in the new scene

    [HideInInspector]
    [SerializeField]
    public float currentTime; // Current time in the world
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