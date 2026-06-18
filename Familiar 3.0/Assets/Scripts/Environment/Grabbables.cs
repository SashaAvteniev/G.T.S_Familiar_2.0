using System.Runtime.CompilerServices;
using UnityEngine;

public class Grabbables : Interactables
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
        readyToInteract = false;
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
