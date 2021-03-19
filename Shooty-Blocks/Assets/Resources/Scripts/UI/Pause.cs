using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to pause and unpause the game
public class Pause : MonoBehaviour
{
    public bool m_paused;

    public void PauseGame()
    {
        m_paused = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        m_paused = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
