using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataScript", menuName = "Scriptable Objects/PlayerDataScript")]
public class PlayerDataScript : ScriptableObject
{
    public PlayerData PlayerData;
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
}