using UnityEngine;

public class Talisman : InteractParent
{ 
    public PlayerData.ETalismans talismanVersion;

    protected override void Interact()
    {
        GameManager.gameData.playerData.currentTalisman = talismanVersion;
        GameManager.gameData.interactFlag = false;
        gameObject.SetActive(false);
    }
}
