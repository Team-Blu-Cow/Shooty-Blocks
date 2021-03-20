using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to pause and unpause the game
public class Pause : MonoBehaviour
{
    public void PauseGame()
    {
        GameController.Instance.paused = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        GameController.Instance.paused = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void ExitLevel()
    {
        GameController.Instance.ExitLevel();
    }
}
