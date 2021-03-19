using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private string m_applicationPath;
    private UserData m_userData;
    
    public int m_speedUpgrades = 0; // Variable to display to player how many times firing speed has been upgraded
    public int m_powerUpgrades = 0; // Variable to display to player how many times firing power has been upgraded


    // Start is called before the first frame update
    private void Start()
    {
        m_applicationPath = Application.persistentDataPath;
        m_userData = new UserData(m_applicationPath);
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
}

