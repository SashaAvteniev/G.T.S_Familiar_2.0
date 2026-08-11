using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    //Serialized
    [SerializeField, Header("Camera")] private float followDistance;
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothSpeed;

    // [Header("Starting Rotation")]
    // //settings for the camera
    // [SerializeField] private float startingPitch;
    // [SerializeField] private float startingYaw;
    
    //holds changes to x & y rotation
    private float currentPitch;
    private float currentYaw;
    
    public float SmoothSpeed { get { return smoothSpeed; } set { smoothSpeed = value; } }

    void Start()
    {
        currentPitch = transform.rotation.eulerAngles.x;
        currentYaw = 0f;
        
        //apply correct camera angle on cat
        // transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
    }
    
    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            currentYaw += 90f;
            target.transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentYaw -= 90f;
            target.transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        }
        
        // keep it in 0-360 range if you want
        currentYaw = Mathf.Repeat(currentYaw, 360f);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("Yaw 1: " + transform.rotation.eulerAngles);
        // base offset
        Vector3 offset = new Vector3(followDistance + 1f, followDistance * 0.75f, 0f);
        
        Debug.Log("Yaw 2: " + transform.rotation.eulerAngles);

        // Rotate the offset around the player
        offset = Quaternion.Euler(0f, currentYaw, 0f) * offset;
        
        Debug.Log("Yaw 3: " + transform.rotation.eulerAngles);
        
        // Vector3 desiredPosition = target.transform.position + offset;
        //
        // transform.position = desiredPosition;
        
        // transform.rotation = Quaternion.Euler(0f, currentYaw, 0f);
        
        // Same Lerp style as your original code
        transform.position = Vector3.Lerp(
            transform.position, 
            target.transform.position + offset, 
            Time.deltaTime * smoothSpeed);
        
        Debug.Log("Yaw 4: " + transform.rotation.eulerAngles);
        
        // Keep the camera looking at the player
        transform.LookAt(target.transform.position + Vector3.up * 1.2f);
        
        Debug.Log("Yaw 5: " + transform.rotation.eulerAngles);
    }
}
