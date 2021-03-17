using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to swap between the canvases that are active
public class CanvasManager : MonoBehaviour
{

    [SerializeField] Canvas in_options;
    [SerializeField] Canvas in_upgrades;
    [SerializeField] Canvas in_menu;
    [SerializeField] Canvas in_menuSelect;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {       
        Debug.Log("Game Start " + GetComponentInChildren<Scrolling>().m_level);
    }

    public void OpenMenu()
    {
        CloseAll();
        in_menu.gameObject.SetActive(true);
    }

    public void ToggleUpgrades()
    {
        bool open = in_upgrades.isActiveAndEnabled;
        CloseAll();
        if (open)
        {
            OpenMenu();
        }
        else
        {
            in_upgrades.gameObject.SetActive(true);
        }
    }

    public void ToggleOptions()
    {
        bool open = in_options.isActiveAndEnabled;
        CloseAll();
        if (open)
        {
            OpenMenu();
        }
        else
        {
            in_options.gameObject.SetActive(true);
        }
    }

    void CloseAll()
    {
        in_options.gameObject.SetActive(false);
        in_upgrades.gameObject.SetActive(false);
        in_menu.gameObject.SetActive(false);
    }
}
