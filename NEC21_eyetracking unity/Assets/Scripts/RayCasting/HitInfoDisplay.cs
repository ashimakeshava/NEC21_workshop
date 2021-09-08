using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitInfoDisplay : MonoBehaviour
{
    public Text GameObjectNameText;

    public Text HitPositionText;

    public Text TimeStampText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetGameObjectNameText(string Text)
    {
        GameObjectNameText.text = Text;
    }

    public void SetHitPositionText(Vector3 position)
    {
        HitPositionText.text = position.ToString();
    }

    public void UpdateTimeStamp()
    {

        TimeStampText.text = TimeManager.Instance.GetCurrentUnixTimeStampString();
    }
}
