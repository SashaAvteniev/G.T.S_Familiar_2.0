using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    bool canInteract = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.eKey.wasPressedThisFrame && canInteract)
        {
            Debug.Log("Open UI");
            // Add other functionality here
        }
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            canInteract = true;
    }

    protected virtual void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            canInteract = false;
    }
}
