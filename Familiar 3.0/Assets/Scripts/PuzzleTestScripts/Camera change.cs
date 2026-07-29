using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class Camerachange : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
   
    //trigger volume
    [SerializeField] private Transform box;
    private Bounds boxBounds;
    
    //starting camera rotation
    private Vector3 baseCameraRotation;
    
    //current camera rotation while transitioning
    private Vector3 currentCameraRotation;
    
    //has the player entered the trigger volume?
    bool hasGoneIn;

    void Start()
    {
        hasGoneIn = false;

        mainCamera.SetActive(true);
        baseCameraRotation = mainCamera.transform.eulerAngles;
        currentCameraRotation = baseCameraRotation;
        //Debug.Log(baseCameraRotation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Player inside trigger volume, switch to puzzle cam
        if (Contains(player.transform.position))
        {
            mainCamera.GetComponent<CameraFollow>().enabled = false;
            mainCamera.GetComponent<PuzzleCamera>().enabled = true;
            currentCameraRotation = mainCamera.transform.eulerAngles;
            hasGoneIn = true;
        }
        //Player not box, switch to normal cam
        else
        {
            if (hasGoneIn)
            {
                mainCamera.GetComponent<PuzzleCamera>().enabled = false;
                mainCamera.GetComponent<CameraFollow>().enabled = true;
                
                //smoothly rotate cam back to original angle
                if(currentCameraRotation.x <= baseCameraRotation.x-.1)
                {
                    mainCamera.GetComponent<CameraFollow>().SmoothSpeed = 3;
                    currentCameraRotation = Vector3.Lerp(currentCameraRotation, baseCameraRotation, Time.deltaTime * 3);
                    mainCamera.transform.eulerAngles = currentCameraRotation;
                    Debug.Log(currentCameraRotation);
                }
                else
                {
                    //reset normal following speed
                    currentCameraRotation.x = baseCameraRotation.x;
                    Debug.Log("hit");
                    mainCamera.GetComponent<CameraFollow>().SmoothSpeed = 10;
                }
            }
        }
    }

    /// <summary>
    ///  check if object has entered trigger volume
    /// </summary>
    /// <param name="position">object position</param>
    /// <returns></returns>
    private bool Contains(Vector3 position)
    {
        Vector3 center = box.transform.position;
        Vector3 scale = box.transform.localScale;
        
        if (position.x < center.x - scale.x / 2 ||
            position.x > center.x + scale.x / 2 ||
            position.y < center.y - scale.y / 2 ||
            position.y > center.y + scale.y / 2 ||
            position.z < center.z - scale.z / 2 ||
            position.z > center.z + scale.z / 2) { return false; }
        
        return true;
    }
}
