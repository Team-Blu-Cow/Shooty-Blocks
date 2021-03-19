using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyImage : MonoBehaviour
{
    private GameController m_gameController;
    // Start is called before the first frame update
    private void Start()
    {
        m_gameController = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        int power = m_gameController.overallPower;
        int levelNum = m_gameController.m_level;      
        int avgBlock = (int)Mathf.Lerp(5 * (levelNum), (5 * levelNum) * 2, 0.5f); 
        
        if(power > avgBlock + 1)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Menu/difficulty EASY");
        }
        else if (power < (avgBlock + 1) && power > (avgBlock - 1))
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Menu/difficulty MEDIUM");
        }
        else if (power < avgBlock - 1)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Menu/difficulty HARD");
        }
    }
}
