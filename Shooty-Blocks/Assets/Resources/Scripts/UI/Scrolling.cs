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

    CanvasManager m_canvasManager;

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_level = GameController.Instance.m_level;
        
    }

    // Start is called before the first frame update
    void Start()
    {        
       m_canvasManager = FindObjectOfType<CanvasManager>();
       SetLevels();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void SetLevels()
    {
        int count = -2;
        m_rectTransform.localPosition = new Vector3(0, 0, 0);

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            text.transform.parent.GetComponent<Image>().enabled = true;
            text.enabled = true;

            if (m_level + count > 0 && m_level+count <= GameController.Instance.m_maxLevel && m_canvasManager.LevelCompleteList.Count > m_level + count)
            {                
                text.text = (m_level + count).ToString();
                text.transform.parent.GetChild(1).gameObject.SetActive(m_canvasManager.LevelCompleteList[m_level + count]);
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

        m_canvasManager.SetLevelCurrency();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        for (int i = 1; i < m_canvasManager.Menu.transform.GetChild(3).childCount; i++)
        {
            m_canvasManager.Menu.transform.GetChild(3).GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        for (int i = 1; i < m_canvasManager.Menu.transform.GetChild(3).childCount; i++)
        {
            m_canvasManager.Menu.transform.GetChild(3).GetChild(i).gameObject.SetActive(true);
        }

        if (m_rectTransform.localPosition.x < -GameController.Instance.m_scrollNextPos && m_level < GameController.Instance.m_maxLevel )
        {
            m_level++;
            SetLevels();
            AudioManager.instance.Play("Scroll Right");

        }
        else if (m_rectTransform.localPosition.x > GameController.Instance.m_scrollNextPos && m_level > 1)
        {
            m_level--;
            SetLevels();
            AudioManager.instance.Play("Scroll Left");
        }
        else
        {
            m_rectTransform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
