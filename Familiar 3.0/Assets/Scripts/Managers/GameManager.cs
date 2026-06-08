using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] public List<Shovables> shovables = new List<Shovables>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shovables.Clear();
        Array gameObjects = GameObject.FindGameObjectsWithTag("Shovable");
        foreach (GameObject gameObject in gameObjects)
        {
            shovables.Add(gameObject.GetComponent<Shovables>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckShoving();
    }

    private void CheckShoving()
    {
        if (player.Shoving)
        {
            foreach (Shovables shovable in shovables)
            {

                if (shovable.ReadyToShove)
                {
                    shovable.ShoveSpeed = player.ShoveSpeed;
                    Debug.Log("shoving!");
                    shovable.Shove();
                }
                else
                {
                    Debug.Log("notCloseEnough");
                }
            }
            player.Shoving = false;
        }
    }
}
