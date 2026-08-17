using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    //Serialized
    [SerializeField, Header("Camera")] private float followDistance;
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;

    // [Header("Starting Rotation")]
    // //settings for the camera
    // [SerializeField] private float startingPitch;
    // [SerializeField] private float startingYaw;
    
    //holds changes to x & y rotation
    private float currentPitch;
    private float currentYaw;
    private Quaternion startingRot;
    private Vector3 baseOffset;
    private Collider leftCollider;
    private Collider rightCollider;
    
    public float SmoothSpeed { get { return smoothSpeed; } set { smoothSpeed = value; } }

    void Start()
    {
        currentPitch = transform.rotation.eulerAngles.x;
        currentYaw = 0f;
        startingRot = target.transform.rotation;
        //apply correct camera angle on cat
        // transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        baseOffset = offset;
        leftCollider = GetComponentsInChildren<Collider>()[0];
        rightCollider = GetComponentsInChildren<Collider>()[1];
    }
    
    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            TurnCameraLeft();
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TurnCameraRight();
        }
        currentYaw = Mathf.Repeat(currentYaw, 360f);
        offset = Quaternion.Euler(0f, currentYaw, 0f) * baseOffset;
    }

    public void TurnCameraRight()
    {
        currentYaw += 90f;
        target.transform.rotation = Quaternion.Euler(startingRot.eulerAngles.x, 
            currentYaw - 90f, startingRot.eulerAngles.z);
    }

    public void TurnCameraLeft()
    {
        currentYaw -= 90f;
        target.transform.rotation = Quaternion.Euler(startingRot.eulerAngles.x,
            currentYaw - 90f, startingRot.eulerAngles.z);
    }

    private void OnValidate()
    {
        if(target)
            transform.position = target.transform.position + offset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position, 
            target.transform.position + offset, 
            Time.deltaTime * smoothSpeed);
        
        // Keep the camera looking at the player
        transform.LookAt(target.transform.position + Vector3.up * 1.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject openObject;
        //bool leftOverlap = Physics.CheckBox()
    }
}
