using UnityEngine;

public class Shovables : Interactables
{
    //Needs to be able to tell when player is nearby
    //Tell something (game manager) that it is ready to be shoved
    //Game manager tells UI to show "Shove"
    //Player will shove it
    //Game Manager sends signal here telling it to shove itself, including player data

    private Vector3 shoveDirection;

    protected float shoveSpeed;

    public float ShoveSpeed
    {
        get { return shoveSpeed; }
        set { shoveSpeed = value; } 
    }
    void Start()
    {
        shoveDirection = Vector3.zero;
        shoveSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }
    protected override void OnCollisionEnter(Collision collision)
    {

        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                readyToInteract = true;
                shoveDirection = contactPoint.normal;
            }
        }
    }

    protected void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                shoveDirection = contactPoint.normal;
            }
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            readyToInteract = false;
        }
    }

    public virtual void Shove()
    {
        Debug.Log("Shoved");
        GetComponent<Rigidbody>().AddForce(shoveDirection*shoveSpeed, ForceMode.Force);
    }
}
