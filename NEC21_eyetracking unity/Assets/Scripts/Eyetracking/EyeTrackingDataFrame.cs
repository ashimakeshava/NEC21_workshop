using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class EyeTrackingDataFrame
{
   //Unix Time Stamp

   public double timestamp;
   
   //HMD Data
   public Vector3 hmdPosition;      //be careful for serialization, Vector3 might not be serializable with your approach
   public Vector3 hmdRotation;
   public Vector3 noseVector; //transport.forward
   
   //Gaze Validity - Data Cleanse - Flags interpreted as bit Mask
   //validity left
   public ulong leftValidityMask;
   public bool leftDataGazeOriginValidity; /** The validity of the origin of gaze of the eye data */
   public bool leftDataGazeDirectionValidity; /** The validity of the direction of gaze of the eye data */
   public bool leftDataPupilDiameterValidity; /** The validity of the diameter of gaze of the eye data */
   public bool leftDataEyeOpennessValidity; /** The validity of the openness of the eye data */
   public bool leftDataPupilPositionInSensorAreaValidity;  /** The validity of normalized position of pupil */
   //public ulong bitmask; // should be order as above (LSB top, MSB bottom) 

   //validity right
   public ulong rightValidityMask;
   public bool rightDataGazeOriginValidity; /** The validity of the origin of gaze of the eye data */
   public bool rightDataGazeDirectionValidity; /** The validity of the direction of gaze of the eye data */
   public bool rightDataPupilDiameterValidity; /** The validity of the diameter of gaze of the eye data */
   public bool rightDataEyeOpennessValidity; /** The validity of the openness of the eye data */
   public bool rightDataPupilPositionInSensorAreaValidity;  /** The validity of normalized position of pupil */
   //LeftEye
   public Vector3 eyePositionLeftLocal;
   public Vector3 eyeDirectionLeftLocal;
   public Vector3 eyePositionLeftWorld;
   public Vector3 eyeDirectionLeftWorld;
   public float eyeOpennessLeft;
   public float eyePupilDiameterLeft;
   
   //rightEye
   public Vector3 eyePositionRightLocal;
   public Vector3 eyeDirectionRightLocal;
   public Vector3 eyePositionRightWorld;
   public Vector3 eyeDirectionRightWorld;
   public float eyeOpennessRight;
   public float eyePupilDiameterRight;
   
   //combined Gaze- " combined eye" - cyclope eye
   public Vector3 EyePositionCombinedLocal;
   public Vector3 EyeDirectionCombinedLocal;
   public Vector3 EyePositionCombinedWorld;
   public Vector3 EyeDirectionCombinedWorld;

   public HitObjectInfo singleHitInfo;
   //Raycast Data normaly done just with the Gaze instead of single eye
   public List<HitObjectInfo> hitInfos;
}



[Serializable] public class  HitObjectInfo
{
   public Vector3 hitPosition;
   public Vector3 ObjectPosition;
   public string ObjectName;
}

[Serializable]
public class Validty
{
   public bool val;
}


