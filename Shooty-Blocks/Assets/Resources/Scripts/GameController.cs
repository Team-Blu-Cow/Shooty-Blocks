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
        m_speedUpgrades++; // Increment the ammount of speed upgrades the ship has
    }

    public void UpgradeBulletPower()
    {
        m_powerUpgrades++; // Increment the ammount of power upgrades the ship has       
    }

    public void ChangeScene()
    {
        m_level = FindObjectOfType<Scrolling>().m_level;

        // Send hook to game analytics
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level" + m_level);
        m_levelLoad.SwitchScene("Level");
    }
}

