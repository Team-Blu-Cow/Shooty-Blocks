using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Block : MonoBehaviour
{
    public bool test = false;

    private float m_fallSpeed = 1f;
    private int m_hp = 0;
    private float m_screenHeight;

    private TMPro.TextMeshPro m_text;
    private Transform renderTransform;

    [SerializeField] private GameObject m_particleExplosion;

    public int hp
    {
        set {m_hp = value; m_text.text = value.ToString();}
        get { return m_hp; }
    }

    public float size
    {
        set { Resize(value); }
        get { return renderTransform.localScale.x; }
    }

    public float fallSpeed
    {
        set { m_fallSpeed = value; }
    }

    public float screenHeight
    {
        set { m_screenHeight = value; }
    }


    private void Resize(float in_scale)
    {
        renderTransform.localScale = new Vector3(in_scale, in_scale, in_scale);
        renderTransform.localPosition = new Vector3(0.5f * in_scale, -0.5f * in_scale, 0);

        RectTransform textTransform = m_text.rectTransform;
        textTransform.localPosition = new Vector3(0.5f * in_scale, -0.5f * in_scale, 0);
    }

    // returns true when block is dead
    public bool Damage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Instantiate(m_particleExplosion, new Vector3(transform.position.x + 0.5f, transform.position.y - 0.5f, transform.position.z), Quaternion.identity);
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public void Awake()
    {
        m_text = GetComponentInChildren<TMPro.TextMeshPro>();
        renderTransform = GetComponentInChildren<SpriteRenderer>().transform;
        m_text.text = m_hp.ToString();
    }

    void Update()
    {
        if(!test)
            transform.position -= new Vector3(0, m_fallSpeed*Time.deltaTime, 0);

        if (transform.position.y < m_screenHeight)
            OnReachBottom();
    }

    void OnBecameInvisible()
    {
        OnReachBottom();
    }

    void OnReachBottom()
    {
        Destroy(gameObject);
    }

}
