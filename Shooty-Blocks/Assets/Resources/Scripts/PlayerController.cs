using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public MasterInput m_inputManager;
    [SerializeField] private bool clicked = false;
    [SerializeField] private int firingPower;
    [SerializeField] private float firingSpeed;

    void Awake()
    {
        m_inputManager = new MasterInput();

        m_inputManager.BasicKBM.LClick.performed += ctx => OnMouseLeftClick();
        m_inputManager.BasicKBM.LClick.canceled += ctx => StopMouseLeftClick();
        m_inputManager.BasicKBM.MousePos.performed += ctx => OnMousePos(ctx.ReadValue<Vector2>());
    }

    void OnEnable()
    {
        m_inputManager.Enable();
    }

    void OnDisable()
    {
        m_inputManager.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Queue<Vector2> m_mousePos = new Queue<Vector2>();
    Vector2 recentPos = Vector2.zero;
    Vector2 pastPos = Vector2.zero;
    void OnMousePos(Vector2 in_mousePos)
    {
 

        m_mousePos.Enqueue(in_mousePos);
        if(m_mousePos.Count > 1)
        {
            recentPos = m_mousePos.Dequeue();
            pastPos = m_mousePos.Dequeue();
        }

        if(clicked == true)
        {
            Vector2 diff = pastPos - recentPos;
            transform.position += new Vector3(diff.x * Time.deltaTime, 0, 0);
            Debug.Log("Difference in movement " + diff);
            Debug.Log("Squeek Squeek");
        }
    }

    void OnMouseLeftClick()
    {
        Debug.Log("Clickety Clack");
        clicked = true;
    }

    void StopMouseLeftClick()
    {
        clicked = false;
    }
}
