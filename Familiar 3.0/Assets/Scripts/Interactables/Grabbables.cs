using System.Runtime.CompilerServices;
using UnityEngine;

public class Grabbables : InteractParent
{
    private bool grabbed;
    private Rigidbody rigidBody;

    void Start()
    {
        // GetComponent<>() is expensive, so we call it once and store the result
        rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // If the player is holding the item, then follow the players position
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
