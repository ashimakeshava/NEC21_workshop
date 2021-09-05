using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    public bool ShowHit;
    public bool drawRay;
    public float maxDistance = 3f;
    public GameObject origin;
    public GameObject TargetEffect;
    public LineRenderer lineRenderer;


    private bool hitTarget;
    private List<GameObject> hitTargets;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hitInfo;

        if (Physics.Raycast(origin.transform.position, origin.transform.up, out hitInfo, maxDistance))
        {
            hitTarget = true;
        }
        else
        {
            hitTarget = false;
        }
        
        
        
        
        //render Ray
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
