using UnityEngine;
using UnityEngine.SceneManagement;

public class Doors : Interactables
{
    [SerializeField] private int scene;
    [SerializeField] private PlayerDataScript playerData;

    [SerializeField] private Transform returnPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enter()
    {
        if(playerData.PlayerData.currentDoor == Vector3.zero)
        {
            SceneManager.LoadScene(scene);
            playerData.PlayerData.currentDoor = returnPosition.position;
            playerData.PlayerData.enteredDoor = true;
        }
        else
        {
            playerData.PlayerData.enteredDoor = false;
            SceneManager.LoadScene(scene);
        }
    }
}
