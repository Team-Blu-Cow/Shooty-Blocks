using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public MasterInput m_inputManager;
    [HideInInspector] private bool m_clicked = false;
    [HideInInspector] private float m_timer;

    [Header ("Player Upgrade Variables")]
    [SerializeField][Range(1, 100)] private int m_firingPower = 1;
    [SerializeField][Range(1,100)] private float m_firingSpeed = 10.0f;

    [SerializeField] private GameObject m_bullet;


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
        transform.position = new Vector3(transform.position.x, -4.0f, transform.position.z);
        float fireTime = 1 / m_firingSpeed;
        m_timer += Time.deltaTime;

        if (m_timer > fireTime)
        {
            Instantiate(m_bullet, new Vector3(transform.position.x, (transform.position.y + 0.75f), 0), Quaternion.identity);
            m_timer = 0.0f;
        }
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

        if(m_clicked == true)
        {
            Vector2 diff = pastPos - recentPos;
            GetComponent<Rigidbody2D>().velocity = new Vector2(diff.x / 2, 0);
            Debug.Log("Squeek Squeek");
        }
    }

    void OnMouseLeftClick()
    {
        Debug.Log("Clickety Clack");
        m_clicked = true;
    }

    void StopMouseLeftClick()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        m_clicked = false;
    }
}
