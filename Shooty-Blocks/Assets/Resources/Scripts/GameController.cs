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
    
    public int m_speedUpgrades = 0; // Variable to display to player how many times firing speed has been upgraded
    public int m_powerUpgrades = 0; // Variable to display to player how many times firing power has been upgraded
    [SerializeField] [Range(10, 110)] private int m_playerPower = 10; // How strong each bullet is
    [SerializeField] [Range(5, 15)] private float m_playerSpeed = 1; // How often a bullet is fired per second

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

    public int m_level;
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
    }

    public void UpgradeBulletSpeed()
    {
        m_playerSpeed += 0.5f;
        m_speedUpgrades++; // Increment the ammount of speed upgrades the ship has
    }

    public void UpgradeBulletPower()
    {
        m_playerPower += 1;
        m_powerUpgrades++; // Increment the ammount of power upgrades the ship has       
    }

    public void ChangeScene()
    {
        // Send hook to game analytics
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level" + m_level);
        m_levelLoad.SwitchScene("Level");
    }
}

