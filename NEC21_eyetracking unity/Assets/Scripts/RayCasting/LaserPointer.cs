using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    public bool drawRay;
    public bool RayCastAll;
    public bool showInfo;
    
    public float maxDistance = 3f;
    public GameObject origin;
    public GameObject TargetEffect;
    public LineRenderer lineRenderer;
    public TVScreen TVScreen;

    private bool hitTarget;
    private List<GameObject> hitTargets;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hitInfo =new RaycastHit();
        RaycastHit[] hitInfos = new RaycastHit[] { };

        if (!RayCastAll)
        {
            
            if (Physics.Raycast(origin.transform.position, origin.transform.up, out hitInfo, maxDistance))
            {
                hitTarget = true;
            }
            else
            {
                hitTarget = false;
            }
        }
        else
        {
            hitInfos = Physics.RaycastAll(origin.transform.position, origin.transform.up, maxDistance);
            
            //sort hit infos by distance
            
            if (hitInfos.Length != 0)
            {
                hitTarget=true;
                
                //Order by distance
                hitInfos = hitInfos.OrderBy(x => x.distance).ToArray();
                hitInfo = hitInfos[hitInfos.Length - 1];
            }
            else
            {
                hitTarget = false;
            }
            
            
        }






        //render Ray - only for cosmetics not needed for understanding
        lineRenderer.gameObject.SetActive(drawRay);
        TargetEffect.SetActive(drawRay);
        if (drawRay)
        {
            if (hitTarget)
            {
                DrawRay(origin.transform.position, hitInfo.point, true);
            }
            else
            {
                DrawRay(origin.transform.position, origin.transform.position+ origin.transform.up*maxDistance, false);
            }
        }
        
        //show information on Screen - only for cosmetics not needed for understanding

        if (showInfo)
        {
            if (hitTarget)
            {
                if (RayCastAll)
                {
                    Debug.Log(hitInfos.Length);
                    TVScreen.DisplayInfo(hitInfos.ToList());
                }
                    
                else
                {
                    TVScreen.DisplayInfo(hitInfo);
                }  
            }
        }
    }


    void DrawRay(Vector3 originPositon, Vector3 hitPosition, bool contactPoint)
    {
        Vector3[] positions = new [] {originPositon,hitPosition};
        
        lineRenderer.SetPositions(positions);

        if (contactPoint)
        {
            TargetEffect.SetActive(true);
            TargetEffect.transform.position = hitPosition;
        }
        else
        {
           TargetEffect.SetActive(false);
        }
    }
}
