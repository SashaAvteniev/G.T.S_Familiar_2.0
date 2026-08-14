using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Fields
    [SerializeField] private float movementSpeed;
    private float speedDefault;
    [SerializeField] private float gravity;
    //[SerializeField] private SpriteRenderer playerSprite;

    private Vector3 direction;
    private Vector3 velocityHorizontal;
    private Vector3 velocityVertical;

    private Vector2 rawInput;
    
    //Jumping
    [SerializeField] private float jumpHeight;
    private float holdJumpTime;
    private bool holdingJump;
    private bool grounded;
    private bool jumped;
    
    //Shoving
    [SerializeField] private float shoveSpeed;
    public float ShoveSpeed { get { return shoveSpeed; } }
    private CharacterController characterController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialize
        grounded = true;
        velocityVertical = Vector3.zero;
        velocityHorizontal = Vector3.zero;
        speedDefault = movementSpeed;
        jumped = false;
        characterController = GetComponent<CharacterController>();
        if(GameManager.gameData.doorExits.ContainsKey(GameManager.gameData.newDoorGUID))
        {
            CharacterController characterController = GetComponent<CharacterController>();
            characterController.enabled = false;
            gameObject.transform.position = GameManager.gameData.doorExits[GameManager.gameData.newDoorGUID];
            characterController.enabled = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        #region calculate velocity
        UpdateDirectionWithCamera();
        velocityHorizontal = new Vector3(direction.x * movementSpeed, 0, direction.z * movementSpeed);
        ApplyGravity();
        #endregion
        #region apply velocity
        characterController.Move(velocityVertical * Time.deltaTime + velocityHorizontal * Time.deltaTime);
        CheckFallingOffEdge();
        CheckLanded();
        #endregion
    }
    
    private void UpdateDirectionWithCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;
    
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
    
        Vector3 cameraRight = mainCamera.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();
    
        direction = (cameraForward * rawInput.y + cameraRight * rawInput.x).normalized;
    }

    #region Player Methods
    //Active player methods
    public void Move(InputAction.CallbackContext context)
    {
        rawInput = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started && grounded)
        {
            velocityVertical = Vector3.up * jumpHeight;
            jumped = true;
            grounded = false;
        }
        else if (GameManager.gameData.playerData.currentTalisman == PlayerData.ETalismans.Elk && context.started && jumped)
        {
            velocityVertical.y = Vector3.up.y * jumpHeight;
            grounded = false;
            jumped = false;
        }
    }
    #endregion



    #region background methods
    //Background methods
    private void ApplyGravity()
    {
       velocityVertical += Time.deltaTime * gravity * Vector3.down;
    }

    private void CheckLanded()
    {
        if (characterController.isGrounded)
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
        if (!characterController.isGrounded && grounded)
        {
            velocityVertical = Vector3.zero;
            gravity = 20;
            grounded = false;
        }
    }
    #endregion
}
