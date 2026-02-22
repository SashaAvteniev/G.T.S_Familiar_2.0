using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Fields
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    [SerializeField] private GameObject player;

    private Vector3 direction;
    private Vector3 velocity;
    private bool grounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grounded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity = new Vector3(direction.x*movementSpeed, direction.y, direction.z*movementSpeed);
        ApplyGravity();
        player.transform.position = player.transform.position + (direction * movementSpeed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
       direction.z = context.ReadValue<Vector2>().x;
       direction.x = -context.ReadValue<Vector2>().y;
       direction = direction.normalized;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            grounded = false;
            direction.y = player.transform.up.y * jumpHeight;
        }
    }
    
    private void ApplyGravity()
    {
        if(!grounded)
        {
            direction.y -= gravity * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            grounded = true;
            direction.y = 0;
            Debug.Log("hit");
        }
    }
}
