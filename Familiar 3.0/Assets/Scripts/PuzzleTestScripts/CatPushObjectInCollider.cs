using UnityEngine;
using UnityEngine.InputSystem;

public class CatPushObjectInCollider : MonoBehaviour
{
    [SerializeField] private SphereCollider catRange;
    private GameObject currentInRangeObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        currentInRangeObject = other.gameObject;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (currentInRangeObject != null && currentInRangeObject.GetComponent<PuzzleObjectPushScript>() != null )
        {
            //currentInRangeObject.GetComponent<PuzzleObjectPushScript>().Push();

        }
    }
}
