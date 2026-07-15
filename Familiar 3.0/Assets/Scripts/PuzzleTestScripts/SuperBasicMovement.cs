using UnityEngine;
using UnityEngine.InputSystem;

public class SuperBasicMovement : MonoBehaviour
{

    [Header("Movement")]
    private float movementSpeed;
    [SerializeField] private float walkSpeed;
    private Vector2 movementDirection;
    [SerializeField] private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementSpeed = walkSpeed;
        Vector3 vecToNextStep = movementDirection * movementSpeed * Time.deltaTime;
        Vector2 nextStep = rb.position + vecToNextStep;
        rb.position = nextStep;
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementDirection = ctx.ReadValue<Vector2>();

    }
}
