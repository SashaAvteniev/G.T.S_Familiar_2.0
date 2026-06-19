using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BackButton : MonoBehaviour
{
    public Button backButton;
    
    //menu player is going to
    public GameObject currentMenu;
    //menu player wants to return to
    public GameObject returnMenu;

    // Update is called once per frame
    void Update()
    {
        backButton.onClick.AddListener(GoBack);
    }

    //hides current window and shows previous window
    void GoBack()
    {
        currentMenu.SetActive(false);
        returnMenu.SetActive(true);
    }
}
