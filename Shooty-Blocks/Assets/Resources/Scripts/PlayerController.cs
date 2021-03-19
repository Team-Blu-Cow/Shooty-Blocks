using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public MasterInput m_inputManager;

    [Header("Player Upgrade Variables")]
    private int m_firingPower = 0; // How strong each bullet is
    private float m_firingSpeed = 0; // How often a bullet is fired per second
    [SerializeField] [Range(1, 3)] private float m_movementSpeed = 1.5f;

    [Header("Projectile Prefab")]
    [SerializeField] private GameObject m_bullet; // Unfortunately, this is click and drag for inspector, This tells what projectile should be fired

    private Rigidbody2D rb;
    private bool m_clicked = false; // This varuable tracks PC controls, to move left click and drag
    private float m_timer; // Timer for firing bullets

    private TMPro.TextMeshPro m_text;

    private void Awake()
    {
        m_inputManager = new MasterInput();

        m_inputManager.BasicKBM.LClick.performed += ctx => OnMouseLeftClick();
        m_inputManager.BasicKBM.LClick.canceled += ctx => StopMouseLeftClick();
        m_inputManager.BasicKBM.MousePos.performed += ctx => OnMousePos(ctx.ReadValue<Vector2>());
        m_inputManager.BasicKBM.FingerTouch.performed += ctx => OnFingerPos(Vector2.zero);

        m_text = GetComponentInChildren<TMPro.TextMeshPro>();
    }

    private void OnEnable()
    {
        m_inputManager.Enable();
    }

    private void OnDisable()
    {
        m_inputManager.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_firingPower = GameController.Instance.firePower;
        m_firingSpeed = GameController.Instance.fireSpeed;
        //m_text.text = (m_gameManager.GetComponent<GameController>().m_speedUpgrades + m_gameManager.GetComponent<GameController>().m_powerUpgrades).ToString();
        m_text.text = m_firingPower.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        // Debug.Log("Firing speed: " + m_firingSpeed);
        // Debug.Log("Firing power: " + m_firingPower);
        float fireTime = 1.0f / m_firingSpeed; // Turns the firing power into a measure of time for how often a bullet should be fired
        m_timer += Time.deltaTime; // Time since last bullet was fired

        if (m_timer > fireTime) // If it is time to fire
        {
            AudioManager.instance.Play("Shoot");
            Instantiate(m_bullet, new Vector3(transform.position.x, (transform.position.y + 0.75f), 0), Quaternion.identity); // Spawn a bullet
            m_timer = 0.0f; // Make timer back to 0 for next bullet to be fired
        }

        if (Input.touchCount > 0) // If there is a finger touching the screen
        {
            Touch touch = Input.GetTouch(0); // Get the touch of the first finger
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            transform.position = new Vector3(touchPos.x, transform.position.y, transform.position.z); // Set the velocity to be the difference in distance of the finger positions (past and current frame)
        }
        else if (m_clicked == false) // If there is no finger movement or pc movement then
        {
            rb.velocity = Vector2.zero; // Set the velocity to be zero
        }

        if (m_clicked == true) // If left mouse is held down
        {
            rb.velocity -= (rb.velocity / 2); // Slow down the velocity. This is so that the player doesn't slide about the place
        }

        if (transform.position.x < -3)
        {
            transform.position = new Vector3(-2.25f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 3)
        {
            transform.position = new Vector3(2.25f, transform.position.y, transform.position.z);
        }
    }

    private Queue<Vector2> m_mousePos = new Queue<Vector2>(); // Queue for recent and last pointer positions (Mouse)
    private Vector2 recentPos = Vector2.zero;
    private Vector2 pastPos = Vector2.zero;

    private void OnMousePos(Vector2 in_mousePos)
    {
        m_mousePos.Enqueue(in_mousePos); // Add most recent pos to queue
        if (m_mousePos.Count > 1) // If the queue's size is greater than 1. This if statement is here so that there is no error in first frame of game
        {
            //Camera.main.ScreenToWorldPoint(touch.position);
            recentPos = Camera.main.ScreenToWorldPoint(m_mousePos.Dequeue()); // Set recent pos to be first value of queue
            pastPos = Camera.main.ScreenToWorldPoint(m_mousePos.Dequeue()); // Set past pos to be second value of queue
        }

        if (m_clicked == true) // If player is clicking right now
        {
            Vector2 diff = pastPos - recentPos; // Calculate difference in positions
            //rb.velocity = new Vector2(diff.x * m_movementSpeed, 0); // Set velocity to what the difference was in positions (divided by a half to slow down movement)
            transform.position = new Vector3(transform.position.x + diff.x, transform.position.y, transform.position.z);
        }
    }

    // Don't think this is needed anymore. Discuss with team
    // TODO @Sandy Reduce function size by using delta in input manager editor
    private Queue<Vector2> m_fingerPos = new Queue<Vector2>(); // Queue for recent and last pointer position (Touch)

    private void OnFingerPos(Vector2 in_fingerPos) // This function is same as above, but for touch controls.
    {
        Debug.Log("Finger Touch");
        //m_fingerPos.Enqueue(in_fingerPos);
        //if (m_fingerPos.Count > 1)
        //{
        //    recentPos = m_fingerPos.Dequeue();
        //    pastPos = m_fingerPos.Dequeue();
        //}

        //Vector2 diff = pastPos - recentPos;
        //rb.velocity = new Vector2(diff.x / 2, 0);
    }

    private void OnMouseLeftClick()
    {
        m_clicked = true; // Set clicking to true
    }

    private void StopMouseLeftClick()
    {
        rb.velocity = Vector2.zero; // Set velocity to 0 as mouse is not dragging across screen
        m_clicked = false; // Set clicking to false
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            
            GameController.Instance.ExitLevel();
            GameController.Instance.m_levelLoad.SwitchScene("MainMenu");
        }
    }
}