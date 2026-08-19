using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Header("Camera")] private GameObject target;
    [FormerlySerializedAs("smoothSpeed")] [SerializeField] private float lerpSpeed = 1f;
    [SerializeField] private Vector3 offset;
    

    private void Start()
    {
        if (target == null) return;

        transform.position = GetDesiredPosition();
        transform.rotation = target.transform.rotation;
    }

    private void Update()
    {
        if (Keyboard.current?.qKey.wasPressedThisFrame == true)
            TurnCameraLeft();

        if (Keyboard.current?.eKey.wasPressedThisFrame == true)
            TurnCameraRight();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = GetDesiredPosition();
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation
            ,Quaternion.Euler(transform.rotation.eulerAngles.x, target.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z),
            Time.deltaTime * lerpSpeed);
    }

    private Vector3 GetDesiredPosition()
    {
        return target.transform.position + offset;
    }

    public void TurnCameraRight()
    {
        if (target == null) return;
        target.transform.Rotate(0f, 90f, 0f, Space.World);
        offset = Quaternion.Euler(0f, -90f, 0f) * offset;
    }

    public void TurnCameraLeft()
    {
        if (target == null) return;
        target.transform.Rotate(0f, -90f, 0f, Space.World);
        offset = Quaternion.Euler(0f, 90f, 0f) * offset;
    }

    private void OnValidate()
    {
        if (target == null) return;

        transform.position = GetDesiredPosition();
        transform.rotation = target.transform.rotation;
    }
}