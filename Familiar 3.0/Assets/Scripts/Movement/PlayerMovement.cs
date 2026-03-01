using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Fields
    [SerializeField] private float movementSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private GameObject player;
    [SerializeField] private bool stag;

    private Vector3 direction;
    private Vector3 velocity;

    //Jumping
    [SerializeField] private float jumpHeight;
    private float holdJumpTime;
    private bool holdingJump;
    private bool grounded;

    private Talisman currentActive;
    enum Talisman
    {
        Elk,
        Badger,
        Sheep,
        Snake
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grounded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region calculate velocity
        velocity = new Vector3(direction.x*movementSpeed, direction.y, direction.z*movementSpeed);
        ApplyGravity();
        #endregion
        #region apply velocity
        player.transform.position = player.transform.position + (direction * movementSpeed * Time.deltaTime);
        #endregion

        if (stag)
        {
            currentActive = Talisman.Elk;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
       direction.z = context.ReadValue<Vector2>().x;
       direction.x = -context.ReadValue<Vector2>().y;
       direction = direction.normalized;
    }

    #region Jumping
    public void Jump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            if(currentActive == Talisman.Elk)
            {

            }
        }
    }
    
    private void ApplyGravity()
    {
        if(!grounded)
        {
            direction.y -= gravity * Time.deltaTime;
        }
    }
    #endregion
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
