using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Fields
    [SerializeField] private float movementSpeed;
    private float speedDefault;
    [SerializeField] private float gravity;
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer playerSprite;

    private Vector3 direction;
    private Vector3 velocityHorizontal;
    private Vector3 velocityVertical;

    //Jumping
    [SerializeField] private float jumpHeight;
    private float holdJumpTime;
    private bool holdingJump;
    private bool grounded;

    //Collisions
    private Vector3 currentWallNormal;

    //Interacting
    private bool interacting;
    public bool Interacting { get { return interacting; } set { interacting = value; } }

    //Shoving
    [SerializeField] private float shoveSpeed;
    public float ShoveSpeed { get { return shoveSpeed; } }
    private bool shoving;
    public bool Shoving { get { return shoving; } set { shoving = value;} }

    //Grabbing
    private bool grabbing;
    public bool Grabbing { get { return grabbing; } set { grabbing = value;} }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize
        grounded = true;
        velocityVertical = Vector3.zero;
        velocityHorizontal = Vector3.zero;
        shoving = false;
        grabbing = false;
        interacting = false;
        speedDefault = movementSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region calculate velocity
        velocityHorizontal = new Vector3(direction.x * movementSpeed, 0, direction.z * movementSpeed);
        ApplyGravity();
        HandleWallCollision();
        #endregion
        #region apply velocity
        player.transform.position = player.transform.position + velocityHorizontal * Time.deltaTime + velocityVertical*Time.deltaTime;
        #endregion

    }

    #region Player Methods
    //Active player methods
    public void Move(InputAction.CallbackContext context)
    {

        direction.z = context.ReadValue<Vector2>().x;
       direction.x = -context.ReadValue<Vector2>().y;
       direction = direction.normalized;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.started && grounded)
        {
            velocityVertical.y = Vector3.up.y * jumpHeight;
            grounded = false;
            movementSpeed = movementSpeed * .7f;
        }
    }

    public void Shove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            shoving = true;
            interacting = true;
        }
        if (context.canceled)
        {
            interacting = false;
        }
    }

    public void Grab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interacting = true;
        }
        if (context.canceled)
        {
            interacting = false;
        }

    }

    #endregion



    #region background methods
    //Background methods
    private void ApplyGravity()
    {
        if(!grounded)
        {
            velocityVertical.y -= gravity * Time.deltaTime;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            grounded = true;
            velocityVertical.y = 0;
            Debug.Log("hit");
            movementSpeed = speedDefault;
        }
        foreach(ContactPoint contactPoint in collision.contacts)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                currentWallNormal = contactPoint.normal;
                Debug.Log(currentWallNormal);
            }
            
        }

    }



    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            currentWallNormal = Vector3.zero;
        }
    }

    private void HandleWallCollision()
    {
        if(velocityHorizontal.x > 0)
        {
            velocityHorizontal.x = velocityHorizontal.x - (currentWallNormal.x * -velocityHorizontal.x);
        }
        if (velocityHorizontal.x < 0)
        {
            velocityHorizontal.x = velocityHorizontal.x - (currentWallNormal.x * velocityHorizontal.x);
        }
        if (velocityHorizontal.z > 0)
        {
            velocityHorizontal.z = velocityHorizontal.z - (currentWallNormal.z * -velocityHorizontal.z);
        }
        if (velocityHorizontal.z < 0)
        {
            velocityHorizontal.z = velocityHorizontal.z - (currentWallNormal.z * velocityHorizontal.z);
        }

    }
    #endregion
}
