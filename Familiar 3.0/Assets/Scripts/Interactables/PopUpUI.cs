using UnityEngine;

public class PopUpUI : InteractParent
{
    //UI that pops up
    [SerializeField] private GameObject popUpUI;
    
    // The main interact method that should be overriden in every single interact object
    protected override void Interact()
    {
        //shows UI
        popUpUI.SetActive(true);
    }
}
