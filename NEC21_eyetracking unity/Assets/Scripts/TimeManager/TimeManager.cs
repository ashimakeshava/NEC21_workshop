using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    
    private double _applicationStartTime;
    
    public double GetCurrentUnixTimeStamp()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }

    public string GetCurrentUnixTimeStampString()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).Hours + ":" + (System.DateTime.UtcNow - epochStart).Minutes +
               ":" + (System.DateTime.UtcNow - epochStart).Seconds + ":" +
               (System.DateTime.UtcNow - epochStart).Milliseconds;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);       
        }
        else
        {
            Destroy(gameObject);
        }
        
        _applicationStartTime = GetCurrentUnixTimeStamp();
    }
}
