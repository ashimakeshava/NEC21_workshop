using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVScreen : MonoBehaviour
{
    private Dictionary<string, GameObject> hitObjects;

    [SerializeField] private GameObject entryTemplate;
    


    [SerializeField]private GameObject TitleRow;
    // Start is called before the first frame update 
    
    [SerializeField]private float Offset;
    private float _offsetStep;
    void Start()
    {
        _offsetStep = Offset;
        hitObjects = new Dictionary<string, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisplayInfo(RaycastHit hitInfo)
    {
        if (hitObjects.ContainsKey(hitInfo.collider.name))
        {
            UpdateInfo(hitObjects[hitInfo.collider.name], hitInfo);
        }
        else
        {
            GameObject entry = GameObject.Instantiate(TitleRow, this.gameObject.transform);
            entry.transform.transform.localPosition = TitleRow.transform.localPosition - Vector3.up * Offset;
            
            Offset+=_offsetStep;
            entry.GetComponent<HitInfoDisplay>().SetHitPositionText(hitInfo.point);
            entry.GetComponent<HitInfoDisplay>().SetGameObjectNameText(hitInfo.collider.name);
            entry.GetComponent<HitInfoDisplay>().UpdateTimeStamp();

            

            hitObjects.Add(hitInfo.collider.name,entry);

            
            
        }
    }
    public void DisplayInfo(List<RaycastHit> hitInfos)
    {

        foreach (var raycastHitInfo in hitInfos)
        {
            if (hitObjects.ContainsKey(raycastHitInfo.collider.name))
            {
                UpdateInfo(hitObjects[raycastHitInfo.collider.name], raycastHitInfo);
            }
            else
            {
                GameObject entry = GameObject.Instantiate(TitleRow, this.gameObject.transform);
                entry.transform.transform.localPosition = TitleRow.transform.localPosition - Vector3.up * Offset;
                
               
                entry.GetComponent<HitInfoDisplay>().SetHitPositionText(raycastHitInfo.point);
                entry.GetComponent<HitInfoDisplay>().SetGameObjectNameText(raycastHitInfo.collider.name);
                entry.GetComponent<HitInfoDisplay>().UpdateTimeStamp();
                
                hitObjects.Add(raycastHitInfo.collider.name,entry);
                Offset+=_offsetStep;
            }
        }
        
    }

    private void UpdateInfo(GameObject gameObjectInfo, RaycastHit hitInfo)
    {
        gameObjectInfo.GetComponent<HitInfoDisplay>().SetHitPositionText(hitInfo.point);
        gameObjectInfo.GetComponent<HitInfoDisplay>().UpdateTimeStamp();
    }
}
