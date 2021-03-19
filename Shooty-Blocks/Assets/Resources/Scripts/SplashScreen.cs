using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    private LevelLoader levelLoader;

    // Start is called before the first frame update
    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        AudioManager.instance.FadeIn("Main Loop");
        StartCoroutine(InitAudio("MainMenu"));
    }

    private IEnumerator InitAudio(string scene)
    {
        yield return new WaitForSeconds(3);
        levelLoader.SwitchScene(scene);
    }
}