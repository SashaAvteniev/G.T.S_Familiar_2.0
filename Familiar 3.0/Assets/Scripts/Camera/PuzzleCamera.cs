using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class PuzzleCamera : MonoBehaviour
{
    //Serialized
    [SerializeField, Header("Camera")] private float followDistance = 10.0f;
    [SerializeField] private GameObject target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 angleFinal;
    [SerializeField] private float defaultSpeed;
    private Vector3 startAngle;

    private bool active;

    private void Start()
    {
        active = false;
        startAngle = transform.eulerAngles;
        Debug.Log(smoothSpeed);

    }
    private void OnEnable()
    {
        startAngle = transform.eulerAngles;
        smoothSpeed = 3;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.transform.position.x + followDistance, target.transform.position.y,
            target.transform.position.z), Time.deltaTime * smoothSpeed);
        
        if (startAngle.x >= angleFinal.x + .1f)
        {
            startAngle = Vector3.Lerp(startAngle, angleFinal, (Time.deltaTime * smoothSpeed));
            transform.eulerAngles = startAngle;
        }
        else
        {
            smoothSpeed = defaultSpeed;
        }

    }

    private void OnDisable()
    {
        startAngle = transform.eulerAngles;
        smoothSpeed = defaultSpeed;
    }
}
