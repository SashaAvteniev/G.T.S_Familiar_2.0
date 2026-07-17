using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Rendering;

public class Camerachange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform box;
    private Bounds boxBounds;
    private Vector3 baseCameraRotation;

    private Vector3 currentCameraRotation;

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
        if (Contains(player.transform.position))
        {
            mainCamera.GetComponent<CameraFollow>().enabled = false;
            mainCamera.GetComponent<PuzzleCamera>().enabled = true;
            currentCameraRotation = mainCamera.transform.eulerAngles;
            hasGoneIn = true;
        }
        else
        {
            if (hasGoneIn)
            {
                mainCamera.GetComponent<PuzzleCamera>().enabled = false;
                mainCamera.GetComponent<CameraFollow>().enabled = true;
                if(currentCameraRotation.x <= baseCameraRotation.x-.1)
                {
                    mainCamera.GetComponent<CameraFollow>().SmoothSpeed = 3;
                    currentCameraRotation = Vector3.Lerp(currentCameraRotation, baseCameraRotation, Time.deltaTime * 3);
                    mainCamera.transform.eulerAngles = currentCameraRotation;
                    Debug.Log(currentCameraRotation);
                }
                else
                {
                    currentCameraRotation.x = baseCameraRotation.x;
                    Debug.Log("hit");
                    mainCamera.GetComponent<CameraFollow>().SmoothSpeed = 10;
                }

            }
        }
    }

    private bool Contains(Vector3 position)
    {
        Vector3 center = box.transform.position;
        Vector3 scale = box.transform.localScale;
        if(position.x < center.x - scale.x/2)
        {
            return false;
        }
        if (position.x > center.x + scale.x / 2)
        {
            return false;
        }
        if (position.y < center.y - scale.y / 2)
        {
            return false;
        }
        if (position.y > center.y + scale.y / 2)
        {
            return false;
        }
        if (position.z < center.z - scale.z / 2)
        {
            return false;
        }
        if (position.z > center.z + scale.z / 2)
        {
            return false;
        }
        return true;
    }
}
