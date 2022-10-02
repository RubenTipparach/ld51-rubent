using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimesliceSelection : MonoBehaviour
{
    public Slider slider;
    public void OnSelectedValue(float second)
    {
        GameManager.Instance
            .manueverTimeline
            .SetHighlight((int)second);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
