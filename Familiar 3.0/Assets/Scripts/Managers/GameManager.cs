using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] public List<Shovables> shovables = new List<Shovables>();
    [SerializeField] public List<Grabbables> grabbables = new List<Grabbables>();

    private Grabbables currentGrabbedObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shovables.Clear();
        Array shovableObjects = GameObject.FindGameObjectsWithTag("Shovable");
        foreach (GameObject gameObject in shovableObjects)
        {
            shovables.Add(gameObject.GetComponent<Shovables>());
        }
        Array grabbableObject = GameObject.FindGameObjectsWithTag("Grabbable");
        foreach (GameObject gameObject in grabbableObject)
        {
            grabbables.Add(gameObject.GetComponent<Grabbables>());
        }

        currentGrabbedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Shoving)
        {
            CheckShoving();
        }
        if (player.Grabbing)
        {
            CheckGrabbing();
        }
        else if (!player.Grabbing)
        {
            CheckDropping();
        }
    }

    private void CheckShoving()
    {

        foreach (Shovables shovable in shovables)
        {

            if (shovable.ReadyToInteract)
            {
                shovable.ShoveSpeed = player.ShoveSpeed;
                shovable.Shove();
            }
        }
        player.Shoving = false;

    }

    private void CheckGrabbing()
    {
        if(currentGrabbedObject == null)
        {
            foreach (Grabbables grabbable in grabbables)
            {
                if (grabbable.ReadyToInteract)
                {
                    currentGrabbedObject = grabbable;

                    currentGrabbedObject.Grab();
                }
            }
        }
        else
        {
           
            currentGrabbedObject.FollowPossition = player.transform.position;
        }
    }

    private void CheckDropping()
    {
        if(currentGrabbedObject != null)
        {
            currentGrabbedObject.Drop();
            currentGrabbedObject = null;
        }
    }
}
