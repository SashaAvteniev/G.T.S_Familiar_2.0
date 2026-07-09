using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private PlayerDataScript playerData;
    [SerializeField] private Transform spawnPoint;
    private List<GameObject> currentlyColiding = new List<GameObject>();
    private Grabbables currentGrabbedObject;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        currentGrabbedObject = null;
        if(playerData.PlayerData.currentDoor != Vector3.zero && !playerData.PlayerData.enteredDoor)
        {
            player.transform.position = new Vector3(playerData.PlayerData.currentDoor.x, 1.914f, playerData.PlayerData.currentDoor.z);
            playerData.PlayerData.currentDoor = Vector3.zero;
        }
        else if (spawnPoint != null) 
        {
            player.transform.position = spawnPoint.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.Interacting);
        if (player.Interacting)
        {
            OnInteract();
            if (player.Grabbing)
            {

                currentGrabbedObject.FollowPossition = player.transform.position;
            }
            currentlyColiding.Clear();
        }
        else
        {
            if (player.Grabbing)
            {
                player.Grabbing = false;
                currentGrabbedObject.Drop();
                currentGrabbedObject=null;
            }
        }
    }

    private void OnInteract()
    {
        //Debug.Log("Hit");
        if (currentGrabbedObject == null)
        {
            Collider[] newCollisions = Physics.OverlapSphere(player.transform.position, 2);
            Vector3 closestPosition = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
            GameObject currentObject = null;
            foreach (Collider collider in newCollisions)
            {
                if (collider.gameObject.GetComponent<Interactables>() != null)
                {
                    currentlyColiding.Add(collider.gameObject);
                    if ((player.transform.position - collider.gameObject.transform.position).magnitude < closestPosition.magnitude)
                    {
                        closestPosition = player.transform.position - collider.gameObject.transform.position;
                        currentObject = collider.gameObject;
                    }
                }
            }
            if (currentObject != null)
            {
                switch (currentObject.tag)
                {
                    case "Shovable":
                        if (currentObject.GetComponent<Shovables>().ReadyToInteract)
                        {

                            currentObject.GetComponent<Shovables>().ShoveSpeed = player.ShoveSpeed;
                            currentObject.GetComponent<Shovables>().Shove();
                            player.Interacting = false;
                        }
                        break;

                    case "Grabbable":
                        
                        if (currentObject.GetComponent<Grabbables>().ReadyToInteract)
                        {
                            
                            currentGrabbedObject = currentObject.GetComponent<Grabbables>();
                            player.Grabbing = true;
                            currentGrabbedObject.Grab();
                            Debug.Log(currentGrabbedObject);
                        }

                        break;

                    case "Talisman":
                        if (currentObject.GetComponent<Talismans>().ReadyToInteract)
                        {
                            currentObject.GetComponent<Talismans>().OnPickup();
                            player.Interacting = false;
                        }
                        break;
                    case "Door":
                        if (currentObject.GetComponent<Doors>().ReadyToInteract)
                        {
                            currentObject.GetComponent<Doors>().Enter();
                            player.Interacting = false;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
