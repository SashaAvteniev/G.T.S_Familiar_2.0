using NUnit.Framework;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Serialized
    [SerializeField, Header("Camera")]
    private float followDistance = 10.0f;
    [SerializeField]
    private float followSpeed = 5.0f;
    
    //Private
    private Camera cameraRef;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraRef = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance =  Vector3.Distance(cameraRef.transform.position, transform.position);
        Vector3 targetPos = cameraRef.transform.position + Vector3.up * followDistance;
        cameraRef.transform.position = Vector3.Lerp(cameraRef.transform.position, targetPos, Time.deltaTime * followDistance * followSpeed);
        
    }
}
