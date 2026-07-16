using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleObjectPushScript : Shovables
{
    Vector3 staringPOS;
    Vector3 startingRotation;
    [SerializeField] int noteValue;
    [SerializeField] Transform target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       staringPOS = this.transform.position;
        startingRotation = this.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void Push()
    //{
    //this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, -4f);
    //}

    public override void Shove()
    {
        GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
        GetComponent<BoxCollider>().center = Vector3.zero;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().AddForce( (target.position - this.transform.position) * shoveSpeed, ForceMode.VelocityChange);
    }
    public void Reset()
    {
        this.gameObject.transform.position = staringPOS;
        this.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.transform.eulerAngles = startingRotation;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public int NoteValue { get { return noteValue; } }
}
