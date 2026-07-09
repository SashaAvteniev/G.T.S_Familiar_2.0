using Unity.VisualScripting;
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
    private bool jumped;

    //Collisions
    private Vector3 currentWallNormal;
    private Vector3 currentFloorNormal;

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

    //PlayerSaveData
    [SerializeField] private PlayerDataScript playerDataScript;

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
        jumped = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region calculate velocity
        velocityHorizontal = new Vector3(direction.x * movementSpeed, 0, direction.z * movementSpeed);
        ApplyGravity();
        #endregion
        #region apply velocity
        player.GetComponent<Rigidbody>().linearVelocity = velocityHorizontal + velocityVertical;
        //CheckLanded();
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
        
        if(context.started)
        {
            velocityVertical = Vector3.up * jumpHeight;
            jumped = true;
            grounded = false;
        }
        else if(playerDataScript.PlayerData.currentTalisman == PlayerData.TalismanInUse.Elk && context.started && jumped)
        {
            velocityVertical.y = Vector3.up.y * jumpHeight;
            grounded = false;
            jumped =false;
        }
    }


    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interacting = true;
        }
        if(context.canceled)
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
            velocityVertical += Vector3.down * gravity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        foreach (ContactPoint contact in collision.contacts)
        {
            if(contact.normal == Vector3.up)
            {
                if (!grounded)
                {
                    
                    velocityVertical = Vector3.zero;
                    grounded = true;
                    movementSpeed = speedDefault;
                    jumped = false;
                    currentFloorNormal = Vector3.up;
                }
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            if(contact.normal == Vector3.up)
            {
                grounded = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        grounded = false;
    }
    #endregion
}
