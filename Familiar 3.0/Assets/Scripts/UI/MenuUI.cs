using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuUI : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool paused;

    void Start()
    {
        //temp since playtester info at start
        SetPaused(true);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(paused);
        //for some reason paused ends up as true after I close the welcome page
        //the player is still allowed to move even though it thinks paused = true
        //the only way to get this if to work is if it's "&& paused"
        //but when it works you go to control, and then go back to pause menu all the panels appear
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !paused)
        {
            Debug.Log("if paused = " + paused);
            SetPaused(true);
            pauseMenu.SetActive(true);
        }
    }

    public void SetPaused(bool value)
    {
        //Debug.Log("before set val paused = " + paused);
        paused = value;
        //Debug.Log("after set val paused = " + paused);

        // //if pause == true, timeScale = 0.0f (frozen)
        // //if paused == false, timeScale = 1.0f (normal speed)
        Time.timeScale = paused ? 0.0f : 1.0f;
    }
}
