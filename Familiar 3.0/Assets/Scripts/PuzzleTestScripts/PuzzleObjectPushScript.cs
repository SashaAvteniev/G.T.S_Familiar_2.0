using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleObjectPushScript : Shovables
{
    Vector3 staringPos;
    Vector3 startingRotation;
    [SerializeField] int noteValue;
    [SerializeField] Transform target;

    [SerializeField]
    [Tooltip("Sound to play when object hits the piano")]
    private AudioClip soundQueue;

    public int NoteValue { get { return noteValue; } }
    public AudioClip SoundQueue { get {return soundQueue;}}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Save where we are right now for the reset
        staringPos = transform.position;
        startingRotation = transform.rotation.eulerAngles;
        
        // GetComponent<>() is expensive so we call it once per component and store the result
        rigidBody = GetComponent<Rigidbody>();
    }
    
    protected override void Interact()
    {
        shoveSpeed = GameManager.player.ShoveSpeed;
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // Make sure it doesn't get shoved into the object its resting on
        direction.Normalize();
        
        // Allow note to fall
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.AddForce(shoveSpeed * direction, ForceMode.Force);
    }
    
    // Reset the object to the starting pos, and freeze it
    public void Reset()
    {
        transform.position = staringPos;
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        transform.eulerAngles = startingRotation;
        rigidBody.isKinematic = false;
    }    
}
