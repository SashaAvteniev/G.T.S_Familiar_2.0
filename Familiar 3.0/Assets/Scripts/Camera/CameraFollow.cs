using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Serialized
    [SerializeField, Header("Camera")] private float followDistance = 10.0f;
    [SerializeField] private float followSpeed = 5.0f;
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x + followDistance, target.transform.position.y + followDistance,
            target.transform.position.z), Time.deltaTime * smoothSpeed);
    }
}
