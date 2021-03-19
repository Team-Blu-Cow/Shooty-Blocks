using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    private float m_fallSpeed = 1f;
    private float m_screenHeight = -5;

    public float fallSpeed
    {
        set { m_fallSpeed = value; }
    }

    public float screenHeight
    {
        set { m_screenHeight = value; }
    }

    void Update()
    {
        transform.position -= new Vector3(0, m_fallSpeed * Time.deltaTime, 0);

        if (transform.position.y < m_screenHeight)
            Destroy(gameObject);
    }
}
