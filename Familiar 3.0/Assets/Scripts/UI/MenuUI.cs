using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEditor;
using System.Net.NetworkInformation;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    //holds pause menu panel
    public GameObject pauseMenu;
    
    //holds if the gameplay should freeze or not
    //t = freeze, f = don't freeze
    public bool paused;
    
    //holds welcome window that appears for playtesters
    public GameObject startingUI;
    
    //object that stores if the intro window has been shown
    public IntroWindowManager windowCheck;

    //keeps track of which menus the player goes through
    private Stack menuOrder = new Stack();
    
    //built in scene to load to
    [SerializeField] private int sceneToLoad;

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

    public void SceneSwitch()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
