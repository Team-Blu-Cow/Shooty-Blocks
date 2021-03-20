using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Class to create the levels for selecting
public class Scrolling : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{       
    private int m_level;

    RectTransform m_rectTransform;
    bool pointerDown = false;
    bool reset = false;

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
            if (m_level < GameController.Instance.m_maxLevel)
            {
                m_level++; 
                SetLevels();
                AudioManager.instance.Play("Scroll Right");
            }
            else
            {
                m_rectTransform.localPosition = new Vector3(-250, 0, 0);
                GetComponent<ScrollRect>().velocity = Vector2.zero;
            }

        }
        else if (m_rectTransform.localPosition.x > 250)
        {
            if (m_level > 1)
            {
                m_level--;
                SetLevels();
                AudioManager.instance.Play("Scroll Left");
            }
            else
            {
                m_rectTransform.localPosition = new Vector3(250, 0, 0);
                GetComponent<ScrollRect>().velocity = Vector2.zero;
            }
        }

        if (!pointerDown && GetComponent<ScrollRect>().velocity == Vector2.zero && !reset)
        {
            SetLevels();
            GetComponent<ScrollRect>().velocity = Vector2.zero;
            reset = true;
        }
    }

    void SetLevels()
    {
        CanvasManager canvasManager = FindObjectOfType<CanvasManager>();

        int count = -2;
        m_rectTransform.localPosition = new Vector3(0, 0, 0);

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.transform.parent.GetComponent<Image>().enabled = true;
            text.enabled = true;

            if (m_level + count > 0 && m_level+count <= GameController.Instance.m_maxLevel)
            {                
                text.text = (m_level + count).ToString();
                text.transform.parent.GetChild(1).gameObject.SetActive(canvasManager.LevelCompleteList[m_level + count]);
            }
            else
            {
                text.transform.parent.GetComponent<Image>().enabled = false;
                text.enabled = false;
                text.transform.parent.GetChild(1).gameObject.SetActive(false);
            }

            count++;   
        }

        GameController.Instance.m_level = m_level;

        canvasManager.SetLevelCurrency();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
        reset = false;
    }
}
