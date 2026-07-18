using System;
using System.Collections.Generic;
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
    private float fullDaySeconds = 0f;
    private Light sun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sun = directionalLight.GetComponent<Light>();
        fullDaySeconds = dayLength * 60f;
    }

    // Allows us to see all lighting changes in the editor so we don't need to press play
    private void OnValidate()
    {
        if(!simulateTime) { return; }
        float sunRotation = Mathf.Lerp(-90, 270, currentTime/2400f);
        sun = directionalLight.GetComponent<Light>();

        // Actually rotating the sun gmae object
        directionalLight.transform.SetPositionAndRotation(new Vector3(), Quaternion.Euler(sunRotation, directionalLight.transform.rotation.y, directionalLight.transform.rotation.z));
        
        // Fades sun intensity in/out as it rises and sets 
        if(currentTime <= 1200f){ sun.intensity = Mathf.InverseLerp(600f, 1200f, currentTime); }
        else { sun.intensity = 1f - Mathf.InverseLerp(1200f, 1800f, currentTime); }

        // Handle lights turning on and off with day & night

        //Get all exterior lights
        GameObject[] extLights = GameObject.FindGameObjectsWithTag("ExtLight");
        for(int i = 0; i < extLights.Length; i++)
        {
            Light l;
            if (l = extLights[i].GetComponentInChildren<Light>()) 
            {
                if(currentTime <= 1200f){ l.intensity = 1f- Mathf.InverseLerp(600f, 1200f, currentTime); }
                else { l.intensity = Mathf.InverseLerp(1200f, 1800f, currentTime); }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!simulateTime) { return; }

        // Converts real time to game time
        currentTime += 2400f / fullDaySeconds * Time.deltaTime;
        if(currentTime >= 2400) {currentTime = 0;} // Restart the day
        float sunRotation = Mathf.Lerp(-90, 270, currentTime/2400f);
        directionalLight.transform.SetPositionAndRotation(new Vector3(), Quaternion.Euler(sunRotation, directionalLight.transform.rotation.y, directionalLight.transform.rotation.z));

        // Fades sun intensity in/out as it rises and sets 
        if(currentTime <= 1200f) { sun.intensity = Mathf.InverseLerp(600f, 1200f, currentTime); } 
        else {sun.intensity = 1f - Mathf.InverseLerp(1200f, 1800f, currentTime);}

                //Get all exterior lights
        GameObject[] extLights = GameObject.FindGameObjectsWithTag("ExtLight");
        for(int i = 0; i < extLights.Length; i++)
        {
            Light l;
            if (l = extLights[i].GetComponent<Light>()) 
            {
                if(currentTime <= 1200f){ l.intensity = 1f - Mathf.InverseLerp(600f, 1200f, currentTime); }
                else { l.intensity = Mathf.InverseLerp(1200f, 1800f, currentTime); }
            }

        }
    }


    // Returns the current hour (0-23)
    int GetHour()
    {
        string time = currentTime.ToString();
        switch(time.Length)
        {
            case 1: return 0;
            case 3: return time[0];
            case 4: return time[0] + time[1];
            default: return -1;
        }
    }

    int GetMinute()
    {
        string time = currentTime.ToString();
        switch(time.Length)
        {
            case 1: return 0;
            case 2: return (int)currentTime;
            case 3: return time[1] + time[2];
            case 4: return time[2] + time[3];
            default: return -2;
        }
    }
}
