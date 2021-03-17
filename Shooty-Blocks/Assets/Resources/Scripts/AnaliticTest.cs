using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnaliticTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AnalyticsEvent.Custom("Cutsom Event", new Dictionary<string, object>
        {
            { "dataPassed", 0 }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
