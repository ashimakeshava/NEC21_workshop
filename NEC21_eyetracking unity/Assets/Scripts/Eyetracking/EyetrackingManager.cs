using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackingManager : MonoBehaviour
{
    public static EyetrackingManager Instance { get; private set; }
    private EyetrackingDevice _eyetrackingDevice;
    public int SetSampleRate = 90;
    public int SetPenetratedLayers = 2;
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

//        _hmdTransform = Camera.main.transform;
    }

    private void Start()
    {
        _eyetrackingDevice = GetComponent<EyetrackingDevice>(); //Only if you add the Eyetracking device on the same Gameobject!
        _sampleRate = 1/SetSampleRate;
        
        _eyetrackingDevice.SetSampleRate(_sampleRate);
        _eyetrackingDevice.SetHMDTransform(_hmdTransform);
        _eyetrackingDevice.SetPenetratedLayers(SetPenetratedLayers);
    }

    //We use this Update method for Debug purposes, for a real experiment, call those functions in a Experiment Manager
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartEyetrackingRecording();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StopEyetrackingRecording();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCalibration();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            SaveDataToDisk();
        }
        
    }

    public void StartEyetrackingRecording()
    {
        Debug.Log("Start Recording...");
        _eyetrackingDevice.StartRecording();
    }

    public void StopEyetrackingRecording()
    {
        _eyetrackingDevice.StopRecording();
    }

    public void ClearDataFrames()
    {
        _eyetrackingDevice.ClearData();
    }

    public void StartCalibration()
    {
        _eyetrackingDevice.StartCalibration();
    }

    public void StartValidation()
    {
        
    }


    public void SaveDataToDisk()
    {
        _eyetrackingDevice.StopRecording();
        DataSavingManager.Instance.SaveList(_eyetrackingDevice.GetCurrentFrames(), "Test session " + TimeManager.Instance.GetCurrentUnixTimeStamp());
    }
    



}
