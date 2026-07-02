using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleObjectPushScript : Shovables
{
    Vector3 staringPOS;
    [SerializeField] int noteValue;
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
        GetComponent<Rigidbody>().AddForce( -this.transform.forward * shoveSpeed, ForceMode.Force);
    }
    public void Reset()
    {
        this.gameObject.transform.position = staringPOS;
    }

    public int NoteValue { get { return noteValue; } }
}
