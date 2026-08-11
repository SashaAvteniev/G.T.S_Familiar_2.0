using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Serialized
    [SerializeField, Header("Camera")] private float followDistance = 10.0f;
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 offset;

    public float SmoothSpeed { get { return smoothSpeed; } set { smoothSpeed = value; } }

    private void OnValidate()
    {
        if (!target) return;
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x + followDistance + 1 + offset.x,
            target.transform.position.y + followDistance * 0.75f + offset.y,
            target.transform.position.z + offset.z), Time.deltaTime * smoothSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target) return;
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x + followDistance + 1 + offset.x,
            target.transform.position.y + followDistance * 0.75f + offset.y,
            target.transform.position.z + offset.z), Time.deltaTime * smoothSpeed);
    }
}
