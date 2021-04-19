using UnityEngine;
using UnityEngine.UI;

// Script to pause and unpause the game
public class Pause : MonoBehaviour
{
    public void PauseGame()
    {
        GameController.Instance.paused = true;
        transform.GetChild(1).gameObject.SetActive(true);
        GetComponentInChildren<Button>().interactable = false;
    }

    public void ResumeGame()
    {
        GameController.Instance.paused = false;
        transform.GetChild(1).gameObject.SetActive(false);
        GetComponentInChildren<Button>().interactable = true;
    }

    public void ExitLevel()
    {
        GameController.Instance.ExitLevel();
    }
}
