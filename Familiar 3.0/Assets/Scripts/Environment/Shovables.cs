using UnityEngine;

public class Shovables : MonoBehaviour
{
    //Needs to be able to tell when player is nearby
    //Tell something (game manager) that it is ready to be shoved
    //Game manager tells UI to show "Shove"
    //Player will shove it
    //Game Manager sends signal here telling it to shove itself, including player data

    private bool readyToShove;
    private Vector3 shoveDirection;

    private float shoveSpeed;

    public float ShoveSpeed
    {
        get { return shoveSpeed; }
        set { shoveSpeed = value; } 
    }
    void Start()
    {
        shoveDirection = Vector3.zero;
        shoveSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {

        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                readyToShove = true;
                shoveDirection = contactPoint.normal;
            }
        }
        Debug.Log(shoveDirection);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                shoveDirection = contactPoint.normal;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            readyToShove = false;
        }
    }

    public void Shove()
    {
        GetComponent<Rigidbody>().AddForce(shoveDirection*shoveSpeed, ForceMode.Force);
    }
}
