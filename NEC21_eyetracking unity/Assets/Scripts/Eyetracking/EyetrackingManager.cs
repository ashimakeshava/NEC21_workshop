using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackingManager : MonoBehaviour
{
    public static EyetrackingManager Instance { get; private set; }
    private EyetrackingDevice _eyetrackingDevice;
    public int SetSampleRate = 90;
    [SerializeField] private Transform _hmdTransform;
 
    private float _sampleRate;
    
 
    //singleton Pattern (also notice the call in line 7) which allows to find the Instance everywhere in the scene by Using EyetrackingManager.Instance
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void StartEyetrackingRecording()
    {
        
    }

    public void StopEyetrackingRecording()
    {
        
    }

    public void StartCalibration()
    {
        
    }

    public void StopCalibration()
    {
        
    }
    



}
