using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

// Class to create the levels for selecting
public class Scrolling : MonoBehaviour
{    
    public int m_level = 1;

    // Start is called before the first frame update
    void Start()
    {
        SetLevels();
    }

    // Update is called once per frame
    void Update()
    {        
        if (GetComponent<RectTransform>().localPosition.x < -250)
        {
            m_level++; 
            SetLevels();           
        }
        else if (GetComponent<RectTransform>().localPosition.x > 250)
        {
            if (m_level > 1)
            {
                m_level--;
                SetLevels();
            }
            else
            {
                GetComponent<RectTransform>().localPosition = new Vector3(250, 0, 0);
            }
        } 
    }

    void SetLevels()
    {
        int count = -2;
        GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.transform.parent.gameObject.SetActive(true);

            if (m_level + count >= 0)
            {                
                text.text = (m_level + count).ToString();
            }
            
            count++;   
        }        
    }
}
