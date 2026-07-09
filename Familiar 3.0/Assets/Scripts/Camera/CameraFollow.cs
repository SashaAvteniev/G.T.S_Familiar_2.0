using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Serialized
    [SerializeField, Header("Camera")] private float followDistance = 10.0f;
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothSpeed;

    public float SmoothSpeed { get { return smoothSpeed; } set { smoothSpeed = value; } }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x + followDistance, target.transform.position.y + followDistance,
            target.transform.position.z), Time.deltaTime * smoothSpeed);
    }
}
