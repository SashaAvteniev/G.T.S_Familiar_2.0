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
    
    private BoxCollider boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        staringPos = transform.position;
        startingRotation = transform.rotation.eulerAngles;
        boxCollider = GetComponent<BoxCollider>();
        rigidBody = GetComponent<Rigidbody>();
    }

    //public void Push()
    //{
    //this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -4f);
    //}

    protected override void Interact()
    {
        shoveSpeed = GameManager.player.ShoveSpeed;
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        direction.Normalize();
        boxCollider.center = Vector3.zero;
        rigidBody.constraints = RigidbodyConstraints.None;
        rigidBody.AddForce(shoveSpeed * direction, ForceMode.Force);
    }
    
    public void Reset()
    {
        transform.position = staringPos;
        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        boxCollider.center = new Vector3(0, 0, .3241f);
        transform.eulerAngles = startingRotation;
        rigidBody.isKinematic = false;
    }    
}
