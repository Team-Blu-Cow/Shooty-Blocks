using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    private float m_fallSpeed = 1f;
    private float m_screenHeight = -5;

    public SaveData in_saveData = null;
    public int in_coinId = 0;

    [Tooltip("The amount of money gained from picking up a coin")]
    public int in_value = 1;

    [SerializeField] private GameObject child = null;

    public void DestroyFamily()
    {
        GameObject.Destroy(child);
        GameObject.Destroy(this);
    }

    public float fallSpeed
    {
        set { m_fallSpeed = value; }
    }

    public float screenHeight
    {
        set { m_screenHeight = value; }
    }

    private void Update()
    {
        transform.position -= new Vector3(0, m_fallSpeed * Time.deltaTime, 0);

        if (transform.position.y < m_screenHeight)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            in_saveData.SetCoinCollected(in_coinId, true);
            GameController.Instance.userData.money += in_value;
            DestroyFamily();
        }
    }
}