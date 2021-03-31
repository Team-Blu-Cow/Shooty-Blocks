using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GameController : MonoBehaviour
{
    // make game controller a singleton
    private static GameController _Instance;

    public static GameController Instance
    { get { return _Instance; } }

    private void Awake()
    {
        if (_Instance != null && _Instance != this)
            Destroy(this.gameObject);
        else
        {
            _Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private string m_applicationPath = null;

    public string applicationPath
    {
        get
        {
            if (m_applicationPath == null)
            {
                m_applicationPath = Application.persistentDataPath;
            }
            return m_applicationPath;
        }
    }

    private UserData m_userData;

    public UserData userData
    {
        get { return m_userData; }
        set { m_userData = value; }
    }

    [SerializeField] [Range(10, 110)] private int m_playerPower = 10; // How strong each bullet is
    [SerializeField] [Range(1, 10)] private float m_playerSpeed = 1; // How often a bullet is fired per second
    private int m_overallPower = 0;

    public int overallPower
    {
        set { m_overallPower = value; }
        get { return m_overallPower; }
    }

    public int firePower
    {
        set { m_playerPower = value; }
        get { return m_playerPower; }
    }

    public float fireSpeed
    {
        set { m_playerSpeed = value; }
        get { return m_playerSpeed; }
    }

    public int m_upgradeCost;
    [Tooltip("the amount the player's firing speed increases by every upgrade")]
    public float m_speedIncrease;
    [Tooltip("the amount the player's bullet damage increases by every upgrade")]
    public int m_powerIncrease;

    public int m_level = 1;
    public int m_maxLevel;
    public LevelLoader m_levelLoad;

    private bool m_paused;
    public bool paused
    {
        set { m_paused = value; }
        get { return m_paused; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        //m_applicationPath = Application.persistentDataPath;
        m_levelLoad = FindObjectOfType<LevelLoader>();
        m_userData = new UserData();

        m_playerPower += userData.powerUpgrade*m_powerIncrease;
        m_playerSpeed += userData.speedUpgrade*m_speedIncrease;
    }

    // Update is called once per frame
    private void Update()
    {
        m_overallPower = (int)(m_playerPower * m_playerSpeed);
    }

    public void UpgradeBulletSpeed()
    {
        if (userData.money >= m_upgradeCost)
        {        
            m_playerSpeed += m_speedIncrease;
            userData.speedUpgrade++; // Increment the amount of speed upgrades the ship has
            userData.money -= m_upgradeCost;
            userData.WriteToDisk();
            
        }
    }

    public void UpgradeBulletPower()
    {
        if (userData.money >= m_upgradeCost)
        {
            m_playerPower += m_powerIncrease;
            userData.powerUpgrade++; // Increment the amount of power upgrades the ship has
            userData.money -= m_upgradeCost;
            userData.WriteToDisk();
        }
    }

    public void ChangeScene()
    {
        // Send hook to game analytics
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level" + m_level);
        m_levelLoad.SwitchScene("Level");
    }

    public void ExitLevel()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level" + m_level);
        Blocks.BlockSpawner spawner = FindObjectOfType<Blocks.BlockSpawner>();
        userData.WriteToDisk();
        spawner.SaveLevelData();
        spawner.DestroyAllLevelObjects();
    }

    public int CoinsCollectedInLevel(int levelID)
    {
        bool output;
        SaveData levelData = new SaveData(levelID.ToString(), out output);

        //if (!output)
        //    return -1;

        Blocks.Level level = Blocks.BlockSpawner.LoadLevel(levelID);

        if (level == null)
            return -1;

        int coinsCollected = 0;

        for(int i = 0; i < level.m_currencyCount; i++)
        {
            if(levelData.IsCoinCollected(i))
            {
                coinsCollected++;
            }
        }

        return coinsCollected;
    }
}