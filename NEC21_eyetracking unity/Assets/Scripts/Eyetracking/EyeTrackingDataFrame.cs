using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackingDataFrame
{
   //Unix Time Stamp

   public double Timestamp;
   
   //HMD Data
   public Vector3 HMDPosition;      //be careful for serialization, Vector3 might not be serializable with your approach
   public Quaternion HMDRotation;
   public Vector3 NoseVector; //transport.forward
   
   //Gaze Validty - Data Cleanse


   //LeftEye
   public Vector3 localPositionLeft;
   public Vector3 localDirectionLeft;
   public Vector3 globalPositionLeft;
   public Vector3 globalDirectionLeft;
   
   //rightEye
   public Vector3 localPositionRight;
   public Vector3 localDirectionRight;
   public Vector3 globalPositionRight;
   public Vector3 globalDirectionRight;
   
   
   //combined Gaze- " combined eye" - cyclope eye
   public Vector3 localPositionGaze;
   public Vector3 localDirectionGaze;
   public Vector3 globalPositionGaze;
   public Vector3 globalDirectionGaze;
   
   
   //Raycast Data normaly done just with the Gaze instead of single eye
   public List<HitObjectInfo> hitInfos;
}



public class  HitObjectInfo
{
   public Vector3 hitPosition;
   public Vector3 ObjectPosition;
   public string ObjectName;
}
