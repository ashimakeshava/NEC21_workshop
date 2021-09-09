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

    private List<EyeTrackingDataFrame> _eyeTrackingDataFrame;
    //Only for a VIVE Pro EYE! you have to addapt this if you want another form of eyetracker
    public void StartRecording()
    {
        if (_isRecording)
        {
            Debug.LogWarning("Recording is already in progress continue with current Recording");
            return;
        }

        if (!_isCalibrated)
        {
            Debug.LogWarning("The Device is not calibrated, aborted");
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
        SRanipal_Eye_v2.GetVerboseData(out data); //Depending on using Sranipal_eye_v2 or v1 //Here you get the device data

        //fill dataframe  with data from the verbose data
        
       
        var leftEyeData = data.left;
        var rightEyeData = data.left;
        var combinedData = data.combined;
        
        //validty

        frame.leftValidityMask = leftEyeData.eye_data_validata_bit_mask;

        frame.rightValidtyMask = rightEyeData.eye_data_validata_bit_mask;
        
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
        
        //combined eye
        
        Vector3 coordinateAdaptedGazeDirectionCombined = new Vector3(combinedData.eye_data.gaze_direction_normalized.x * -1,  combinedData.eye_data.gaze_direction_normalized.y, combinedData.eye_data.gaze_direction_normalized.z);
        frame.EyePositionCombinedLocal = combinedData.eye_data.gaze_origin_mm;
        frame.EyeDirectionCombinedLocal = coordinateAdaptedGazeDirectionCombined;

        frame.EyePositionCombinedWorld = combinedData.eye_data.gaze_origin_mm / 1000 + _hmdTransform.position;
        frame.EyeDirectionCombinedWorld = _hmdTransform.rotation * coordinateAdaptedGazeDirectionCombined;

        frame.hitInfos =
            GetHitObjects(frame.EyePositionCombinedWorld, frame.EyeDirectionCombinedWorld, _penetratedLayer);
        
        
        _eyeTrackingDataFrame.Add(frame);
        
        yield return new WaitForSeconds(_sampleRate);
        }
    }

    public void StartCalibration()
    {
        if (_isRecording|| _isCalibrating) return;
        _isCalibrating = true; //For Sranipal not needed, since it is pausing unity.... anyway

        _isCalibrated= SRanipal_Eye_v2.LaunchEyeCalibration();
        _isCalibrating = false;
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
            if (i > raycastHits.Length)
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
        _eyeTrackingDataFrame.Clear();
    }
}
