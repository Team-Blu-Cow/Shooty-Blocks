using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Class to create the levels for selecting
public class Scrolling : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{       
    public int m_level;
    public int m_maxLevel;

    RectTransform m_rectTransform;
    bool pointerDown = false;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_level = GameController.Instance.m_level;
    }

    // Start is called before the first frame update
    void Start()
    {        
        SetLevels();
    }

    // Update is called once per frame
    void Update()
    {        
        if (m_rectTransform.localPosition.x < -250)
        {
            if (m_level < m_maxLevel)
            {
                m_level++; 
                SetLevels();
            }
            else
            {
                m_rectTransform.localPosition = new Vector3(-250, 0, 0);
            }

        }
        else if (m_rectTransform.localPosition.x > 250)
        {
            if (m_level > 1)
            {
                m_level--;
                SetLevels();
            }
            else
            {
                m_rectTransform.localPosition = new Vector3(250, 0, 0);
            }
        }

        if (!pointerDown && GetComponent<ScrollRect>().velocity == Vector2.zero)
        {
            SetLevels();
            GetComponent<ScrollRect>().velocity = Vector2.zero;
        }
    }

    void SetLevels()
    {
        int count = -2;
        m_rectTransform.localPosition = new Vector3(0, 0, 0);

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.transform.parent.GetComponent<Image>().enabled = true;
            text.enabled = true;

            if (m_level + count > 0)
            {                
                text.text = (m_level + count).ToString();
            }
            else
            {
                text.transform.parent.GetComponent<Image>().enabled = false;
                text.enabled = false;
            }

            count++;   
        }

        GameController.Instance.m_level = m_level;

        FindObjectOfType<CanvasManager>().SetLevelCurrency();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
    }
}
