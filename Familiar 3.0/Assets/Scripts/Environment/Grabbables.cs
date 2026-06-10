using System.Runtime.CompilerServices;
using UnityEngine;

public class Grabbables : Shovables
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private bool grabbed;
    void Start()
    {
        grabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        readyToInteract = true;
    }


    protected override void OnCollisionExit(Collision collision)
    {
        readyToInteract = false;
    }

    private void Grab(Vector3 position)
    {
        grabbed = true;
        this.transform.position = position;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Drop()
    {
        grabbed = false;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
}
