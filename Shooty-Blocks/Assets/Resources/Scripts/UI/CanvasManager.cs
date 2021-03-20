using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;

// Class to swap between the canvases that are active
public class CanvasManager : MonoBehaviour
{
    // Canvases in the main menu
    [Header("Canvases")]
    [SerializeField] Canvas in_options;
    [SerializeField] Canvas in_upgrades;
    [SerializeField] Canvas in_menu;
    [SerializeField] Canvas in_menuSelect;

    [Header("Misc")]
    [SerializeField] TMPro.TextMeshProUGUI[] in_currencyCounter;
    [SerializeField] TMPro.TextMeshProUGUI[] in_upgradeCounter;
    [SerializeField] TMPro.TextMeshProUGUI[] in_upgradeCosts;
    [SerializeField] TMPro.TextMeshProUGUI in_levelcurrencyCounter;

    [SerializeField] LevelLoader in_levelLoad;
    [SerializeField] Toggle in_toggle;

    private List<int> m_collectedCurrencyList;
    private List<bool> m_levelCompleteList = new List<bool>();
    public List<bool> LevelCompleteList
    {
        get { return m_levelCompleteList; }
    }

    private void Awake()
    {
        // Init game analytics has to be done before any event calls
        GameAnalytics.Initialize();
        GameController.Instance.m_levelLoad = in_levelLoad;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetData();

        in_upgradeCosts[0].text = GameController.Instance.m_upgradeCost.ToString();
        in_upgradeCosts[1].text = GameController.Instance.m_upgradeCost.ToString();

        CloseAll();
        OpenMenu();
    }

    private void Update()
    {
        SetDisplayMoney(GameController.Instance.userData.money);
    }

    public void SetDisplayMoney(int money)
    {
        in_currencyCounter[0].text = money.ToString();
        in_currencyCounter[1].text = money.ToString();
    }

    public void SetLevelCurrency()
    {
        if(m_collectedCurrencyList != null)
        {
            int coinsCollected = m_collectedCurrencyList[GameController.Instance.m_level];

            Blocks.Level level = Blocks.BlockSpawner.LoadLevel(GameController.Instance.m_level);

            string text = (level != null && coinsCollected >= 0) ? coinsCollected.ToString() + "/" + level.currencyCount.ToString() : "???";

            in_levelcurrencyCounter.text = text;
        }
        
    }

    public void StartGame()
    {
        GameController.Instance.ChangeScene();
        AudioManager.instance.Play("Select");
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
        in_upgradeCounter[0].text = GameController.Instance.userData.speedUpgrade.ToString();
    }

    public void OnPowerUpgrade()
    {
        GameController.Instance.UpgradeBulletPower();
        in_upgradeCounter[1].text = GameController.Instance.userData.powerUpgrade.ToString();
    }

    public void SetControlGroup(bool toggle)
    {
        GameController.Instance.userData.controlGroup = toggle;
    }

    public void DestroyGameData()
    {
        GameController.Instance.userData.DestroyDirectory();
        GameController.Instance.userData = new UserData();
        ResetData();
    }

    private void ResetData()
    {
        m_collectedCurrencyList = new List<int>();

        for (int i = 0; i <= GameController.Instance.m_maxLevel; i++)
        {
            int coinsCollected = GameController.Instance.CoinsCollectedInLevel(i);

            m_collectedCurrencyList.Add(coinsCollected);
            SaveData levelSaveData = new SaveData(i.ToString(), out _);
            m_levelCompleteList.Add(levelSaveData.IsLevelComplete());
        }

        in_upgradeCounter[0].text = GameController.Instance.userData.speedUpgrade.ToString();
        in_upgradeCounter[1].text = GameController.Instance.userData.powerUpgrade.ToString();

        GameController.Instance.firePower = 10 + GameController.Instance.userData.powerUpgrade * GameController.Instance.m_powerIncrease;
        GameController.Instance.fireSpeed = 1 + GameController.Instance.userData.speedUpgrade * GameController.Instance.m_speedIncrease;

        in_toggle.isOn = GameController.Instance.userData.controlGroup;
    }
}
