using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public MasterInput m_inputManager;

    public static event Action m_PlayerHasDied;

    [Header("Player Upgrade Variables")]
    private int m_firingPower = 0; // How strong each bullet is

    private float m_firingSpeed = 0; // How often a bullet is fired per second
    [SerializeField] [Range(100, 500)] private float m_movementSpeed = 0.0f;

    [Header("Projectile Prefab")]
    [SerializeField] private GameObject m_bullet; // Unfortunately, this is click and drag for inspector, This tells what projectile should be fired

    private Rigidbody2D rb;
    private bool m_clicked = false; // This varuable tracks PC controls, to move left click and drag
    private float m_timer; // Timer for firing bullets

    private bool m_frozen;

    private TMPro.TextMeshPro m_text;

    private bool dead = false;

    private void Awake()
    {
        m_inputManager = new MasterInput();

        m_inputManager.BasicKBM.LClick.performed += ctx => OnMouseLeftClick();
        m_inputManager.BasicKBM.LClick.canceled += ctx => StopMouseLeftClick();
        m_inputManager.BasicKBM.MousePos.performed += ctx => OnMousePos(ctx.ReadValue<Vector2>());
        m_inputManager.BasicKBM.FingerTouch.performed += ctx => OnFingerPos(ctx);

        m_text = GetComponentInChildren<TMPro.TextMeshPro>();
        initFadeOut();
        m_frozen = false;
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
        m_text.text = m_firingPower.ToString();

        GameController.Instance.freezeDelegate += OnLevelFreeze;
    }

    // Update is called once per frame
    private void Update()
    {
        if (dead || m_frozen)
            return;

        float fireTime = 1.0f / m_firingSpeed; // Turns the firing power into a measure of time for how often a bullet should be fired
        m_timer += Time.deltaTime; // Time since last bullet was fired

        if (m_timer > fireTime) // If it is time to fire & not paused
        {
            AudioManager.instance.Play("Shoot");
            GameObject bullet = Instantiate(m_bullet, new Vector3(transform.position.x, (transform.position.y + 0.75f), 0), Quaternion.identity); // Spawn a bullet
            GameController.Instance.freezeDelegate += bullet.GetComponent<Bullet>().OnLevelFreeze;
            m_timer = 0.0f; // Make timer back to 0 for next bullet to be fired
        }

        //if (rb != null)
        //{
        //    if (Input.touchCount > 0) // If there is a finger touching the screen
        //    {
        //        Debug.Log("update touch");
        //        Touch touch = Input.GetTouch(0); // Get the touch of the first finger
        //        rb.velocity = new Vector2(touch.deltaPosition.x, 0);
        //
        //        //Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
        //        //transform.position = new Vector3(touchPos.x, transform.position.y, transform.position.z); // Set the velocity to be the difference in distance of the finger positions (past and current frame)
        //    }
        //    else if (m_clicked == false) // If there is no finger movement or pc movement then
        //    {
        //        rb.velocity = Vector2.zero; // Set the velocity to be zero
        //    }
        //
        //    if (m_clicked == true) // If left mouse is held down
        //    {
        //        // Essentially drag
        //        rb.velocity -= (rb.velocity / 2); // Slow down the velocity. This is so that the player doesn't slide about the place
        //    }
        //}

        // These if statements make sure the player does not go off screen
        if (transform.position.x < -2.51)
        {
            transform.position = new Vector3(-2.5f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 2.51)
        {
            transform.position = new Vector3(2.5f, transform.position.y, transform.position.z);
        }
    }

    private Queue<Vector2> m_mousePos = new Queue<Vector2>(); // Queue for recent and last pointer positions (Mouse)
    private Vector2 recentPos = Vector2.zero;
    private Vector2 pastPos = Vector2.zero;

    private void OnMousePos(Vector2 in_mousePos)
    {
        return;
        m_mousePos.Enqueue(in_mousePos); // Add most recent pos to queue
        if (m_mousePos.Count > 1) // If the queue's size is greater than 1. This if statement is here so that there is no error in first frame of game
        {
            recentPos = Camera.main.ScreenToWorldPoint(m_mousePos.Dequeue()); // Set recent pos to be first value of queue
            pastPos = Camera.main.ScreenToWorldPoint(m_mousePos.Dequeue()); // Set past pos to be second value of queue
        }

        if (m_clicked == true) // If player is clicking right now
        {
            Vector2 diff = pastPos - recentPos; // Calculate difference in positions
            rb.velocity = new Vector2(diff.x * m_movementSpeed, 0); // Set velocity to what the difference was in positions (divided by a half to slow down movement)
                                                                    //transform.position = new Vector3(transform.position.x + diff.x, transform.position.y, transform.position.z);
        }
    }

    // TODO @Sandy with matthew's new button stuff figure out movement as it doesn't work. The function below might work?
    private Queue<Vector2> m_fingerPos = new Queue<Vector2>(); // Queue for recent and last pointer position (Touch)

    private void OnFingerPos(InputAction.CallbackContext in_context) // This function is same as above, but for touch controls.
    {
        float delta_x = m_inputManager.BasicKBM.FingerTouch.ReadValue<Vector2>().x;
        rb.velocity = new Vector2(delta_x / 2, 0);
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
            DeathSequence(collision.gameObject);
        }
    }

    private void DeathSequence(GameObject block)
    {
        if (!dead)
        {
            StartCoroutine(DeathAnimation(block));
        }
    }

    private void initFadeOut()
    {
        float vertExtent = Camera.main.orthographicSize; // get the camera size in world space
        float horzExtent = vertExtent * Screen.width / Screen.height;

        GameObject BlackBackground = GameObject.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity); // create a new object at origin
        BlackBackground.name = "Fadeout";
        SpriteRenderer SpriteRef = BlackBackground.AddComponent<SpriteRenderer>(); // create and reference a spriterenderer
        SpriteRef.sortingOrder = 999; // move sorting order to max to display infront of all sprites
        SpriteRef.sprite = Resources.Load<Sprite>("Sprites/Background/star_bg");
        SpriteRef.color = new Color(0, 0, 0, 0);
        SpriteRef.size = new Vector2(horzExtent, vertExtent);
    }

    private IEnumerator DeathAnimation(GameObject block)
    {
        GetComponent<SpriteRenderer>().sortingOrder = 1000; // move the player and the colided block infront of the fadeout BG

        block.GetComponent<SpriteRenderer>().sortingOrder = 1000;

        //// WARNING // YOU // ENTERED // DA // MAF // ZONE //
        //float xDist = transform.position.x - block.transform.position.x; // - is left + is right
        //float yDist = transform.position.y - block.transform.position.y; // - is down + is up
        //Vector2 playerToBlockOffset = new Vector2(transform.position.x + (xDist / 2), transform.position.y + (yDist / 2));
        //
        ////Vector2 localOrigin = new Vector2(xDist / 2, yDist / 2);
        //
        //float hyp = Vector3.Distance(transform.position, block.transform.position);
        //// WARNING // YOU // ENTERED // DA // MAF // ZONE //
        dead = true;
        GameObject BBref = GameObject.Find("Fadeout");
        SpriteRenderer SpriteRef = BBref.GetComponent<SpriteRenderer>();
        float safeGuard = 0f;
        float opacity = SpriteRef.color.a;

        float currentTime = 0;
        while (currentTime < 1) // fade the background to black
        {
            currentTime += Time.deltaTime;
            safeGuard += Time.deltaTime;
            SpriteRef.color = new Color(0, 0, 0, currentTime / 1);

            yield return null;
        }
        GameController.Instance.ExitLevel(); // updates anylitics and cleans the blocks in the scene
        GameController.Instance.m_levelLoad.SwitchScene("MainMenu");
        yield break;

        //AudioManager.instance.Play(name);
        //float currentTime = 0;
        //float start = 0f;
        //
        //loopSource.volume = start;
        //yield return new WaitForSeconds(leadInTime);
        //
        //while (currentTime < 1)
        //{
        //    currentTime += Time.deltaTime;
        //    if (start != null)
        //    {
        //        startSource.volume = Mathf.Lerp(start, 1f, currentTime / 1);
        //    }
        //    loopSource.volume = Mathf.Lerp(start, 1f, currentTime / 1);
        //    if (end != null)
        //    {
        //        endSource.volume = Mathf.Lerp(start, 1f, currentTime / 1);
        //    }
        //    yield return null;
        //}
        //yield break;
    }

    public void OnLevelFreeze(bool state)
    {
        m_frozen = state;
    }

    public void OnDestroy()
    {
        GameController.Instance.freezeDelegate -= OnLevelFreeze;
    }
}