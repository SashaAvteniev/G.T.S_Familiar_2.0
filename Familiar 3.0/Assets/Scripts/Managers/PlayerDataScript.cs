using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataScript", menuName = "Scriptable Objects/PlayerDataScript")]
public class PlayerDataScript : ScriptableObject
{
    public PlayerData PlayerData;

    private void OnEnable()
    {
        PlayerData.currentTalisman = PlayerData.TalismanInUse.None;
        PlayerData.currentDoor = Vector3.zero;
        PlayerData.lastKnownY = 0;
        PlayerData.enteredDoor = false;
    }
}

[System.Serializable]
public class PlayerData
{
    public enum TalismanInUse
    {
        None,
        Elk,
        Sheep,
        Snake,
        Badger
    }
    public TalismanInUse currentTalisman;
  

    public Vector3 currentDoor;
    
    public bool enteredDoor;

    public float lastKnownY;
}