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

    private List<GameObject> currentlyColiding = new List<GameObject>();
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
        if(player.Interacting)
        {
            OnInteract();
            currentlyColiding.Clear();
        }
    }

    private void OnInteract()
    {
        Collider[] newCollisions = Physics.OverlapSphere(player.transform.position, 1);
        foreach (Collider collider in newCollisions)
        {
            currentlyColiding.Add(collider.gameObject);
            Debug.Log(collider.gameObject.tag);
        }
    }
}
