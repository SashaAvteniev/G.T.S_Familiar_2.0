using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor;
using System.Net.NetworkInformation;

public class MenuUI : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool paused;
    public GameObject startingUI;
    public IntroWindowManager windowCheck;

    private Stack menuOrder = new Stack();

    void Start()
    {
        paused = true;
        Time.timeScale = 0.0f;
        menuOrder.Push(startingUI);
        
        //Check if the intro window has been shown before
        //If it has, don't show playtester window again
        if (!windowCheck.hasBeenShown)
        {
            windowCheck.hasBeenShown = true;
        }
        else
        {
            startingUI.SetActive(false);
            SetPause(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && !paused)
        {
            SetPause(true);
        }
    }

    public void SetPause(bool value)
    {
        if(value)
        {
            //Freeze time
            Time.timeScale = 0.0f;
            paused = value;
            pauseMenu.SetActive(true);
            menuOrder.Push(pauseMenu.gameObject);
            return;
        }

        paused = value;
        menuOrder.Clear();
        Time.timeScale = 1.0f;
    }

    public void GoForward(GameObject nextUI)
    {
        GameObject top = (GameObject)menuOrder.Peek();
        top.SetActive(false);
        menuOrder.Push(nextUI);
        nextUI.SetActive(true);
    }

    public void GoBack()
    {
        GameObject top = (GameObject)menuOrder.Pop();
        top.SetActive(false);
        top = (GameObject)menuOrder.Peek();
        top.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("THE GAME HAS QUIT");
        Application.Quit();
    }
}
