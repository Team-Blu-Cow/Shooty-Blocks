using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameController gameController = FindObjectOfType<GameController>();

        if (gameController)
        {
            // Set to new scenes objects
            gameController.m_levelLoad = FindObjectOfType<LevelLoader>();
            gameController.GetComponent<Blocks.BlockSpawner>().cameraBounds = FindObjectOfType<Camera>();

            // Build and start level
            gameController.GetComponent<Blocks.BlockSpawner>().BuildLevel(gameController.m_level);
            gameController.GetComponent<Blocks.BlockSpawner>().StartSpawning();
        }
    }
}
