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
        m_gameController = GameController.Instance; // Get the game controller
    }

    // Update is called once per frame
    void Update()
    {
        int power = m_gameController.overallPower; // Get the player's over all power (fire speed * fire power)
        int levelNum = m_gameController.m_level; // Get current level selected
        int avgBlock = (int)Mathf.Lerp(5 * (levelNum), (5 * levelNum) * 2, 0.5f); // Figure out average block health
        
        if(power > avgBlock + 1) // If the player's overall power is better than average hp of a block then display that this level is easy
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Menu/difficulty EASY");
        }
        else if (power < (avgBlock + 1) && power > (avgBlock - 1)) // If the player's overall power is equal to the average hp of a block (+-1) then display that the level is of medium difficulty
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Menu/difficulty MEDIUM");
        }
        else if (power < avgBlock - 1) // If the player's overall power is less than the average hp of a block then display that the level is going to be hard
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Menu/difficulty HARD");
        }
    }
}
