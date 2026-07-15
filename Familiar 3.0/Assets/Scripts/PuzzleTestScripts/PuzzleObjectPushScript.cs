using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleObjectPushScript : Shovables
{
    Vector3 staringPOS;
    [SerializeField] int noteValue;
    [SerializeField] Transform target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       staringPOS = this.transform.position;
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
        GetComponent<Rigidbody>().AddForce( (target.position - this.transform.position) * shoveSpeed, ForceMode.VelocityChange);
        Debug.Log((target.position - this.transform.position) * shoveSpeed);
    }
    public void Reset()
    {
        this.gameObject.transform.position = staringPOS;
        this.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public int NoteValue { get { return noteValue; } }
}
