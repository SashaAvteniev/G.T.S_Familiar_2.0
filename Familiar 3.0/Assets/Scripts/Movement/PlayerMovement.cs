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
        HandleWallCollision();
        #endregion
        #region apply velocity
        player.transform.position = player.transform.position + velocityHorizontal * Time.deltaTime + velocityVertical*Time.deltaTime;
        #endregion

        Debug.Log(currentWallNormal);

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
            jumped = true;
        }
        else if(playerDataScript.PlayerData.currentTalisman == PlayerData.TalismanInUse.Elk && context.started && grounded==false && jumped)
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
        foreach(ContactPoint contactPoint in collision.contacts)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("floor"))
            {
                grounded = true;
                currentFloorNormal = contactPoint.normal;
                velocityVertical.y = 0;
                movementSpeed = speedDefault;
                jumped = false;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                currentWallNormal = contactPoint.normal;
            }
            
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contactPoint in collision.contacts)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("floor"))
            {
                currentFloorNormal = contactPoint.normal;
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
            {
                currentWallNormal = contactPoint.normal;
            }

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            currentWallNormal = Vector3.zero;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("floor"))
        {
            grounded = false;
            currentFloorNormal.y = 0;
        }
    }

    private void HandleWallCollision()
    {
        if(velocityHorizontal.x > 0)
        {
            velocityHorizontal.x = velocityHorizontal.x - (currentWallNormal.x * -velocityHorizontal.x);
            velocityHorizontal.x = velocityHorizontal.x - (currentFloorNormal.x * -velocityHorizontal.x);
        }
        if (velocityHorizontal.x < 0)
        {
            velocityHorizontal.x = velocityHorizontal.x - (currentWallNormal.x * velocityHorizontal.x);
            velocityHorizontal.x = velocityHorizontal.x - (currentFloorNormal.x * velocityHorizontal.x);
        }
        if (velocityHorizontal.z > 0)
        {
            velocityHorizontal.z = velocityHorizontal.z - (currentWallNormal.z * -velocityHorizontal.z);
            velocityHorizontal.z = velocityHorizontal.z - (currentFloorNormal.z * -velocityHorizontal.z);
        }
        if (velocityHorizontal.z < 0)
        {
            velocityHorizontal.z = velocityHorizontal.z - (currentWallNormal.z * velocityHorizontal.z);
            velocityHorizontal.z = velocityHorizontal.z - (currentFloorNormal.z * velocityHorizontal.z);
        }
    }


    #endregion
}
