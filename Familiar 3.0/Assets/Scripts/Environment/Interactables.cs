using UnityEngine;
using UnityEngine.EventSystems;

public class Interactables : MonoBehaviour
{
    [SerializeField]
    protected bool readyToInteract;
    public bool ReadyToInteract { get {  return readyToInteract; } }

    [SerializeField] private float weight;
    public float Weight { get { return weight; } }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    protected virtual void OnCollisionEnter(Collision collision)
    {

        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                readyToInteract = true;
            }
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            readyToInteract = false;
        }
    }

}
