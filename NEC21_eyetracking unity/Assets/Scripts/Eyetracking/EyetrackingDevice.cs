using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;
using ViveSR.anipal.Eye;

public class EyetrackingDevice : MonoBehaviour
{
    private bool _isCalibrating;
    private bool _isCalibrated;
    private bool _isRecording;
    private float _sampleRate;
    private int _penetratedLayer;
    private Transform _hmdTransform;

    [SerializeField] private List<EyeTrackingDataFrame> _eyeTrackingDataFrames;
    
    //Only for a VIVE Pro EYE! you have to addapt this if you want another form of eyetracker

    private void Start()
    {
        _eyeTrackingDataFrames = new List<EyeTrackingDataFrame>();
    }
    
    
    public void StartCalibration()
    {
        if (_isRecording|| _isCalibrating) return;
        _isCalibrating = true; //For Sranipal not needed, since it is pausing unity.... anyway

        _isCalibrated= SRanipal_Eye_v2.LaunchEyeCalibration();
        _isCalibrating = false;
    }
    
    
    public void StartRecording()
    {
        if (_isRecording)
        {
            Debug.LogWarning("Recording is already in progress continue with current Recording");
            return;
        }

       
        
        _isRecording = true;
        
        StartCoroutine(Recording());
    }

    private IEnumerator Recording()
    {
        while (_isRecording)
        {
            EyeTrackingDataFrame frame = new EyeTrackingDataFrame();

            frame.timestamp = TimeManager.Instance.GetCurrentUnixTimeStamp();

            VerboseData data;
            
            
            //HMD Data

            frame.hmdPosition = _hmdTransform.transform.position;
            frame.hmdRotation = _hmdTransform.transform.rotation.eulerAngles;
            frame.noseVector = _hmdTransform.transform.forward;
            
            
            SRanipal_Eye_v2.GetVerboseData(out data); //Depending on using Sranipal_eye_v2 or v1 //Here you get the device data

            //fill dataframe  with data from the verbose data
            
           
            var leftEyeData = data.left;
            var rightEyeData = data.right;
            var combinedData = data.combined;
            
            //validity

            #region validity

            frame.leftValidityMask = leftEyeData.eye_data_validata_bit_mask;

            leftEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_GAZE_ORIGIN_VALIDITY);

            frame.rightValidtyMask = rightEyeData.eye_data_validata_bit_mask;

            #endregion

           
            
            // left eye data
            
            Vector3 coordinateAdaptedGazeDirectionLeft = new Vector3(leftEyeData.gaze_direction_normalized.x * -1,  leftEyeData.gaze_direction_normalized.y, leftEyeData.gaze_direction_normalized.z);
            //local
            frame.eyePositionLeftLocal= leftEyeData.gaze_origin_mm;
            frame.eyeDirectionLeftLocal = coordinateAdaptedGazeDirectionLeft;
            //global
            frame.eyePositionLeftWorld= leftEyeData.gaze_origin_mm / 1000 + _hmdTransform.position;
            frame.eyeDirectionLeftWorld = _hmdTransform.rotation * coordinateAdaptedGazeDirectionLeft;
            
            // Openness and Pupil Diameter
            frame.eyeOpennessLeft = leftEyeData.eye_openness;
            frame.eyePupilDiameterLeft = leftEyeData.pupil_diameter_mm;
            
            //right eye data
            
            Vector3 coordinateAdaptedGazeDirectionRight = new Vector3(rightEyeData.gaze_direction_normalized.x * -1,  rightEyeData.gaze_direction_normalized.y, rightEyeData.gaze_direction_normalized.z);
            frame.eyePositionRightLocal = rightEyeData.gaze_origin_mm;
            frame.eyeDirectionRightLocal = coordinateAdaptedGazeDirectionRight;
            //global
            frame.eyePositionRightWorld = rightEyeData.gaze_origin_mm / 1000 + _hmdTransform.position;
            frame.eyeDirectionRightWorld = _hmdTransform.rotation * coordinateAdaptedGazeDirectionRight;

            // Openness and Pupil Diameter
            frame.eyeOpennessRight = rightEyeData.eye_openness;
            frame.eyePupilDiameterRight = rightEyeData.pupil_diameter_mm;
            
            //combined eye - average the eye position and direction
            
            Vector3 coordinateAdaptedGazeDirectionCombined = new Vector3(combinedData.eye_data.gaze_direction_normalized.x * -1,  combinedData.eye_data.gaze_direction_normalized.y, combinedData.eye_data.gaze_direction_normalized.z);
            frame.EyePositionCombinedLocal = combinedData.eye_data.gaze_origin_mm;
            frame.EyeDirectionCombinedLocal = coordinateAdaptedGazeDirectionCombined;

            frame.EyePositionCombinedWorld = combinedData.eye_data.gaze_origin_mm / 1000 + _hmdTransform.position;
            frame.EyeDirectionCombinedWorld = _hmdTransform.rotation * coordinateAdaptedGazeDirectionCombined;

            if (_penetratedLayer > 1)
            {
                //RaycastAll
                frame.hitInfos =
                    GetHitObjects(frame.EyePositionCombinedWorld, frame.EyeDirectionCombinedWorld, _penetratedLayer);
            }
            else
            {
                //simple Raycast
                
            }
            
            
            
            _eyeTrackingDataFrames.Add(frame);
            yield return new WaitForSeconds(_sampleRate);
        }
    }

    
    
    public void StopRecording()
    {
        _isRecording = false;
    }

    public void SetSampleRate(float sampleRate)
    {
        _sampleRate = sampleRate;
    }

    public void SetHMDTransform(Transform HMDTransform)
    {
        _hmdTransform = HMDTransform;
    }

    public void SetPenetratedLayers(int number)
    {
        _penetratedLayer = number;
    }

    private List<HitObjectInfo> GetHitObjects(Vector3 origin, Vector3 direction, int penetratedLayers=1)
    {
        List<HitObjectInfo> hitObjectInfos = new List<HitObjectInfo>();

        RaycastHit[] raycastHits = Physics.RaycastAll(origin, direction);
        
        
        raycastHits = raycastHits.OrderBy(x=>x.distance).ToArray();

        for (int i = 0; i < penetratedLayers; i++)
        {
            if (i >= raycastHits.Length)
                break;
            
            HitObjectInfo hitObjectInfo = new HitObjectInfo();
            hitObjectInfo.ObjectName = raycastHits[i].collider.name;
            hitObjectInfo.hitPosition = raycastHits[i].point;
            hitObjectInfo.ObjectPosition = raycastHits[i].transform.position;
            hitObjectInfos.Add(hitObjectInfo);
            
        }

        return hitObjectInfos;
    }
    
    


    public void ClearData()
    {
        _eyeTrackingDataFrames.Clear();
    }
}
