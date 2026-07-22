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

    //Interacting
    private bool interacting;
    public bool Interacting { get { return interacting; } set { interacting = value; } }

    //Shoving
    [SerializeField] private float shoveSpeed;
    public float ShoveSpeed { get { return shoveSpeed; } }
    private bool shoving;
    public bool Shoving { get { return shoving; } set { shoving = value; } }

    //Grabbing
    private bool grabbing;
    public bool Grabbing { get { return grabbing; } set { grabbing = value; } }

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
        player.GetComponent<CharacterController>().Move(velocityVertical * Time.deltaTime + velocityHorizontal * Time.deltaTime);
        CheckFallingOffEdge();
        CheckLanded();
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


        if (context.started && grounded)
        {
            velocityVertical = Vector3.up * jumpHeight;
            jumped = true;
            grounded = false;
        }
        else if (playerDataScript.PlayerData.currentTalisman == PlayerData.TalismanInUse.Elk && context.started && jumped)
        {
            velocityVertical.y = Vector3.up.y * jumpHeight;
            grounded = false;
            jumped = false;
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
       velocityVertical += Vector3.down * gravity*Time.deltaTime;
    }

    private void CheckLanded()
    {
        if (player.GetComponent<CharacterController>().isGrounded)
        {
            if (!grounded)
            {
                //Debug.Log("player is grounded");
                velocityVertical = Vector3.zero;
                grounded = true;
                movementSpeed = speedDefault;
                jumped = false;
                gravity = 30;
            }

        }
    }

    private void CheckFallingOffEdge()
    {
        if (!player.GetComponent<CharacterController>().isGrounded && grounded)
        {
            velocityVertical = Vector3.zero;
            gravity = 20;
            grounded = false;
        }
    }
    #endregion
}
