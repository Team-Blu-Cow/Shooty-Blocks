using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Block : MonoBehaviour
{
    public Blocks.BlockType type;

    private float m_fallSpeed = 1f;
    private int m_hp = 0;
    private float m_screenBottom;
    private float m_screenTop;

    private TMPro.TextMeshPro m_text;
    private Transform m_renderTransform;
    private Collider2D m_collider;

    private Color weakColor; // Shows weakest color for the block
    private Color strongColor; // Shows strongest color for the block
    private float enemyColor = 0.0f; // Float value for the weighting of the enemy's color

    [SerializeField] private GameObject m_particleExplosion;

    public int hp
    {
        set { m_hp = value; m_text.text = value.ToString(); }
        get { return m_hp; }
    }

    public float size
    {
        set { Resize(value); }
        get { return m_renderTransform.localScale.x; }
    }

    public float fallSpeed
    {
        set { m_fallSpeed = value; }
    }

    public float screenBottom
    {
        set { m_screenBottom = value; }
    }

    public float screenTop
    {
        set { m_screenTop = value; }
    }

    private void Resize(float in_scale)
    {
        m_renderTransform.localScale = new Vector3(in_scale, in_scale, in_scale);
        m_renderTransform.localPosition = new Vector3(1, -1, 0);

        RectTransform textTransform = m_text.rectTransform;
        textTransform.localPosition = new Vector3(1, -0.8f, 0);
    }

    // returns true when block is dead
    public bool Damage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            //AudioManager.instance.Play("Block Explosion");
            Instantiate(m_particleExplosion, new Vector3(transform.position.x + 0.5f, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
            AudioManager.instance.Play("Block Explosion");
            Destroy(gameObject);
            return true;
        }

        // TODO @Sandy This is quick solution, collapse to function, yessss? (Same block of code in block spawner when spawning block)
        Color weakColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Color strongColor = new Color(0.14f, 0.16f, 0.36f, 1.0f);
        int maxHP = ((5 * (GameController.Instance.m_level + 1) * 2) * 2) * ((type == Blocks.BlockType.LARGE) ? 2 : 1);
        float weighting = (float)hp / (float)maxHP;

        Color blockColor = Color.white;
        blockColor.r = Mathf.Lerp(weakColor.r, strongColor.r, weighting);
        blockColor.g = Mathf.Lerp(weakColor.g, strongColor.g, weighting);
        blockColor.b = Mathf.Lerp(weakColor.b, strongColor.b, weighting);

        Debug.Log("Weighting: " + weighting);
        GetComponentInChildren<SpriteRenderer>().color = blockColor;

        return false;
    }

    public void Awake()
    {
        weakColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        strongColor = new Color(0.14f, 0.16f, 0.36f, 1.0f);

        m_text = GetComponentInChildren<TMPro.TextMeshPro>();
        m_renderTransform = GetComponentInChildren<SpriteRenderer>().transform;
        m_collider = GetComponentInChildren<Collider2D>();
        m_collider.enabled = false;
        m_hp = Random.Range(2, 5);
        m_text.text = m_hp.ToString();
    }

    private void Update()
    {
        if (m_collider.enabled == false && transform.position.y < m_screenTop + ((type == Blocks.BlockType.LARGE)? 2 : 1))
        {
            m_collider.enabled = true;
        }

        transform.position -= new Vector3(0, m_fallSpeed * Time.deltaTime, 0);

        if (transform.position.y < m_screenBottom)
            OnReachBottom();
    }

    private void OnReachBottom()
    {
        DestroyFamily();
    }

    public void DestroyFamily()
    {
        Destroy(m_renderTransform.gameObject);
        Destroy(m_text.gameObject);
        Destroy(gameObject);
    }
}