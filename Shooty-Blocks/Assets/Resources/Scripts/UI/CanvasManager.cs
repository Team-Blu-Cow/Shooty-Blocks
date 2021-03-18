using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

// Class to swap between the canvases that are active
public class CanvasManager : MonoBehaviour
{
    // Canvases in the main menu
    [SerializeField] Canvas in_options;
    [SerializeField] Canvas in_upgrades;
    [SerializeField] Canvas in_menu;
    [SerializeField] Canvas in_menuSelect;

    private void Awake()
    {
        // Init game analytics has to be done before any event calls
        GameAnalytics.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseAll();
        OpenMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {       
        Debug.Log("Game Start " + GetComponentInChildren<Scrolling>().m_level);

        // Send hook to game analytics
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level" + GetComponentInChildren<Scrolling>().m_level);
    }

    public void OpenMenu()
    {
        CloseAll();
        in_menu.gameObject.SetActive(true);
    }

    public void ToggleUpgrades()
    {
        // Get if the upgrades are already open or not
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
        // Get if the options are already open or not
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

    // Close all the canvases
    void CloseAll()
    {
        in_options.gameObject.SetActive(false);
        in_upgrades.gameObject.SetActive(false);
        in_menu.gameObject.SetActive(false);
    }
}
