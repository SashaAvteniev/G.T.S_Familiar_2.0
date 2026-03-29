using UnityEngine;

public class PuzzleObjectPushScript : MonoBehaviour
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

    public void Push()
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, .190f);
    }

    public void Reset()
    {
        this.gameObject.transform.position = staringPOS;
    }
}
