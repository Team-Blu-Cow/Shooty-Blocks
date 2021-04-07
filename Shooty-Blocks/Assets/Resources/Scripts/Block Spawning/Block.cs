using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
        hp -= damage; // Damage the block using the value passed in
        if (hp <= 0) // If hp is less than 0 aka block is dead
        {
            //AudioManager.instance.Play("Block Explosion");
            Instantiate(m_particleExplosion, new Vector3(transform.position.x + 0.5f, transform.position.y - 0.5f, transform.position.z), Quaternion.identity); // Spawn an explosion particle system
            AudioManager.instance.Play("Block Explosion");
            Destroy(gameObject); // Destroy block
            return true;
        }

        ChangeColor(); // Change the color of the block to show that it is now weaker

        return false;
    }

    public void ChangeColor()
    {
        int maxHP = ((5 * (GameController.Instance.m_level + 1) * 2) * 2) * ((type == Blocks.BlockType.LARGE) ? 2 : 1); // Figure out the highest amount of hp a block could have
        float weighting = (float)hp / (float)maxHP; // Figure out what the block's hp is compared to the max hp it could have

        Color bWeakColor = new Color(0.6f, 0.7f, 0.96f, 1.0f); // Create color for a block with not a lot of health (sprite)
        Color bStrongColor = new Color(0.14f, 0.16f, 0.36f, 1.0f); // Create a color for a block with a lot of health (sprite)

        Color blockColor = Color.white; // Create color for block to change to 
        blockColor.r = Mathf.Lerp(bWeakColor.r, bStrongColor.r, weighting); // Lerp with strong and weak color to get the sprite's color
        blockColor.g = Mathf.Lerp(bWeakColor.g, bStrongColor.g, weighting); // Lerp with strong and weak color to get the sprite's color
        blockColor.b = Mathf.Lerp(bWeakColor.b, bStrongColor.b, weighting); // Lerp with strong and weak color to get the sprite's color

        GetComponentInChildren<SpriteRenderer>().color = blockColor; // Set the sprite color to be the lerped values calculated above

        Color tWeakColor = new Color(0.8f, 0.8f, 0.8f, 1.0f); // Create color for a block with not a lot of health (text)
        Color tStrongColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); // Create a color for a block with a lot of health (text)

        Color textColor = Color.white;
        textColor.r = Mathf.Lerp(tWeakColor.r, tStrongColor.r, weighting); // Lerp with strong and weak color to get the text's color
        textColor.g = Mathf.Lerp(tWeakColor.g, tStrongColor.g, weighting); // Lerp with strong and weak color to get the text's color
        textColor.b = Mathf.Lerp(tWeakColor.b, tStrongColor.b, weighting); // Lerp with strong and weak color to get the text's color

        GetComponentInChildren<TextMeshPro>().color = textColor; // Set the text color to be the lerped calues calculated above

    }

    public void Awake()
    {

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