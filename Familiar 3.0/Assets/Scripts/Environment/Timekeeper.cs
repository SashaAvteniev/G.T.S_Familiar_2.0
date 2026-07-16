using System;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Timekeeper : MonoBehaviour
{
    public bool simulateTime = true;
    public GameObject directionalLight;
    
    //0 is midnight, 2300 is 11pm.
    [Range(0, 2400)]
    public float currentTime = 0f;

    [Tooltip("Day & night length (seperatley) in minutes. E.g. a value of 5 would be 5 minutes of daylight, and 5 minutes of nightime.")]
    public float dayLength = 10f;
    private float seconds = 0f;
    private float minuteLength = 0;
    private float fullDaySeconds = 0f;
    private Light sun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sun = directionalLight.GetComponent<Light>();
    }

    private void OnValidate()
    {
        float sunRotation = Mathf.Lerp(-90, 270, currentTime/2400f);
        directionalLight.transform.SetPositionAndRotation(new Vector3(), Quaternion.Euler(sunRotation, directionalLight.transform.rotation.y, directionalLight.transform.rotation.z));
        fullDaySeconds = dayLength * 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!simulateTime) { return; }
        currentTime += 2400f / fullDaySeconds * Time.deltaTime;
        if(currentTime >= 2400) {currentTime = 0;}
        float sunRotation = Mathf.Lerp(-90, 270, currentTime/2400f);
        directionalLight.transform.SetPositionAndRotation(new Vector3(), Quaternion.Euler(sunRotation, directionalLight.transform.rotation.y, directionalLight.transform.rotation.z));
        if(currentTime <= 1200f)
        {
            sun.intensity = Mathf.InverseLerp(600f, 1200f, currentTime);
        } else {sun.intensity = 1f - Mathf.InverseLerp(1200f, 1800f, currentTime);}
        //Debug.Log(sun.intensity);
    }


    // Returns the current hour (0-23)
    float GetHour()
    {
        string time = currentTime.ToString();
        switch(time.Length)
        {
            case 1:
                return 0;
            case 3:
                {
                    return time[0];
                }
            case 4:
                {
                    return time[0] + time[1];
                }
            default:
                {
                    return -1;
                }
        }
    }
}
