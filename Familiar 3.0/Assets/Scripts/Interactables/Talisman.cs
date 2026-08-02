using UnityEngine;

public class Talisman : InteractParent
{
    [SerializeField] PlayerData talismanVersion;
    [SerializeField] GameData playerData;

    protected override void Interact()
    {
        playerData.data.currentTalisman = talismanVersion.currentTalisman;
        gameObject.SetActive(false);
    }
}
