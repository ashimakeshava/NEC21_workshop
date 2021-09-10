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
   
   //Gaze Validty - Data Cleanse - Flags interpreted as bit Mask
   public ulong leftValidityMask;

   public ulong rightValidtyMask;
   
   
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


