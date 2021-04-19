using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class freezeLevelButton : MonoBehaviour
{
    private Button button;
    public bool state;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { GameController.Instance.FreezeButtonPress(state); });
    }
}
