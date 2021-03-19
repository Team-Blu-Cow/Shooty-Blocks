using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set to new scenes objects
        GameController.Instance.m_levelLoad = FindObjectOfType<LevelLoader>();
        GameController.Instance.GetComponent<Blocks.BlockSpawner>().cameraBounds = Camera.main;

        // Build and start level
        GameController.Instance.GetComponent<Blocks.BlockSpawner>().BuildLevel(GameController.Instance.m_level);
        GameController.Instance.GetComponent<Blocks.BlockSpawner>().StartSpawning();
    }
}
