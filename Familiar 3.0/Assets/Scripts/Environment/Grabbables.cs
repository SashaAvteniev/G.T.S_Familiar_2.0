using System.Runtime.CompilerServices;
using UnityEngine;

public class Grabbables : Shovables
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector3 followPossition;
    public Vector3 FollowPossition
    {
        get { return followPossition; }
        set { followPossition = value; }
    }

    private bool grabbed;
    void Start()
    {
        grabbed = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(followPossition);
        if(grabbed)
        {
            this.transform.position = followPossition;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        readyToInteract = true;
    }


    protected override void OnCollisionExit(Collision collision)
    {
        readyToInteract = false;
    }

    public void Grab()
    {
        grabbed = true;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void Drop()
    {
        grabbed = false;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }
}
