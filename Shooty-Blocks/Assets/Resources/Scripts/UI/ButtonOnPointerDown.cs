using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonOnPointerDown : MonoBehaviour, IEndDragHandler, IBeginDragHandler ,IDragHandler
{
    Vector2 m_mousePos;
    Vector3 m_startPos;

    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = transform.parent.GetComponentInParent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Button>().interactable = true;
        GetComponentInParent<Scrolling>().OnPointerUp(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_mousePos = eventData.position;
        m_startPos = rectTransform.position;

        GetComponent<Button>().interactable = false;

        GetComponentInParent<Scrolling>().OnPointerDown(eventData);        
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = m_startPos - new Vector3((m_mousePos.x - eventData.position.x), 0, 0);
    }
}
