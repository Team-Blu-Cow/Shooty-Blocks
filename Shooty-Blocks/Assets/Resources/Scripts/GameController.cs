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

    public int m_level = 1;
    public LevelLoader m_levelLoad;

    // Start is called before the first frame update
    private void Start()
    {
        //m_applicationPath = Application.persistentDataPath;
        m_levelLoad = FindObjectOfType<LevelLoader>();
        m_userData = new UserData();
    }

    // Update is called once per frame
    private void Update()
    {
        m_overallPower = (int)(m_playerPower * m_playerSpeed);
    }

    public void UpgradeBulletSpeed()
    {
        m_playerSpeed += 0.5f;
        userData.speedUpgrade++; // Increment the amount of speed upgrades the ship has
        userData.WriteToDisk();
    }

    public void UpgradeBulletPower()
    {
        m_playerPower += 1;
        userData.powerUpgrade++; // Increment the amount of power upgrades the ship has
        userData.WriteToDisk();
    }

    public void ChangeScene()
    {
        // Send hook to game analytics
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level" + m_level);
        m_levelLoad.SwitchScene("Level");
    }

    public void ExitLevel()
    {
        Blocks.BlockSpawner spawner = FindObjectOfType<Blocks.BlockSpawner>();

        spawner.SaveLevelData();
        spawner.DestroyAllLevelObjects();
    }

    public int CoinsCollectedInLevel()
    {
        bool output;
        SaveData levelData = new SaveData(GameController.Instance.m_level.ToString(), out output);

        Blocks.Level level = Blocks.BlockSpawner.LoadLevel(GameController.Instance.m_level);

        int coinsCollected = 0;

        for(int i = 0; i < level.currencyCount; i++)
        {
            if(levelData.IsCoinCollected(i))
            {
                coinsCollected++;
            }
        }

        return coinsCollected;
    }
}