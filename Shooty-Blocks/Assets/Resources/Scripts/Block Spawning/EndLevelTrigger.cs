using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class EndLevelTrigger : MonoBehaviour
{
    private float m_fallSpeed = 1f;
    private float m_screenHeight = -5;

    public SaveData levelSaveData;
    public Blocks.BlockSpawner blockSpawner;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!GameController.Instance.userData.controlGroup)
            {
                for (int i = 0;i< GameController.Instance.GetComponent<Blocks.BlockSpawner>().CurrencyCount;i++)
                {
                    levelSaveData.SetCoinCollected(i,true);
                    GameController.Instance.userData.money += GameController.Instance.GetComponent<Blocks.BlockSpawner>().CurrencyCount;
                }                
            }            

            if (levelSaveData != null)
            {
                levelSaveData.WriteToDisk();
            }

            blockSpawner.DestroyAllLevelObjects();

            GameController.Instance.userData.WriteToDisk();

            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level" + GameController.Instance.m_level);
            
            FindObjectOfType<LevelLoader>().SwitchScene("MainMenu");

        }
    }
}
