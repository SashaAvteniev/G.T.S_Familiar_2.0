using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Shovables : InteractParent
{
    //Needs to be able to tell when player is nearby
    //Tell something (game manager) that it is ready to be shoved
    //Game manager tells UI to show "Shove"
    //Player will shove it
    //Game Manager sends signal here telling it to shove itself, including player data

    private Vector3 shoveDirection;
    protected float shoveSpeed;
    protected Rigidbody rigidBody;

    public float ShoveSpeed
    {
        get { return shoveSpeed; }
        set { shoveSpeed = value; } 
    }

    void Start()
    {
        shoveDirection = Vector3.zero;
        shoveSpeed = 0;
        rigidBody = GetComponent <Rigidbody>();
    }
    
    
    protected override void Interact()
    {
        shoveSpeed = GameManager.player.ShoveSpeed;
        shoveDirection = transform.position - GameManager.player.transform.position;
        shoveDirection = shoveDirection.normalized;
        shoveDirection.y = 0;
        Debug.Log(shoveDirection); 
        rigidBody.AddForce(shoveDirection*shoveSpeed, ForceMode.Force);
    }
}
