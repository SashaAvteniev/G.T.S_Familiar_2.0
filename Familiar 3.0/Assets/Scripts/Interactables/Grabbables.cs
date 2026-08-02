using System.Runtime.CompilerServices;
using UnityEngine;

public class Grabbables : InteractParent
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private bool grabbed;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(grabbed)
        {
            transform.position = GameManager.player.transform.position;
        }
    }

    // Rewritten to be a toggle instead of hold
    protected override void Interact()
    {
        grabbed = !grabbed;
        if(grabbed)
        {
            rigidBody.isKinematic = true;
            return;
        }
        rigidBody.isKinematic = false;
    }
}
