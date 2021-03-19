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
    [SerializeField] TMPro.TextMeshProUGUI in_currencyCounter;
    [SerializeField] TMPro.TextMeshProUGUI in_levelcurrencyCounter;

    [SerializeField] LevelLoader in_levelLoad;

    private List<int> m_collectedCurrencyList;

    private void Awake()
    {
        // Init game analytics has to be done before any event calls
        GameAnalytics.Initialize();
        GameController.Instance.m_levelLoad = in_levelLoad;
    }

    // Start is called before the first frame update
    void Start()
    {
        

        CloseAll();
        OpenMenu();
    }

    private void Update()
    {
        SetDisplayMoney(GameController.Instance.userData.money);
    }

    public void SetDisplayMoney(int money)
    {
        in_currencyCounter.text = money.ToString();
    }

    public void SetLevelCurrency()
    {
        int coinsCollected = GameController.Instance.CoinsCollectedInLevel();

        string text = coinsCollected.ToString() + "/" + Blocks.BlockSpawner.LoadLevel(GameController.Instance.m_level).currencyCount.ToString();

        in_levelcurrencyCounter.text = text;
    }

    public void StartGame()
    {
        GameController.Instance.ChangeScene();
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

    public void OnSpeedUpgrade()
    {
        GameController.Instance.UpgradeBulletSpeed();
    }

    public void OnPowerUpgrade()
    {
        GameController.Instance.UpgradeBulletPower();
    }
}
