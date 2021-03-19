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

    [SerializeField] private GameObject player; // Variable for game controller to find player
    
    [SerializeField] private int m_speedUpgrades = 1; // Variable to display to player how many times firing speed has been upgraded
    [SerializeField] private int m_powerUpgrades = 1; // Variable to display to player how many times firing power has been upgraded


    // Start is called before the first frame update
    private void Start()
    {

        //m_applicationPath = Application.persistentDataPath;
        m_userData = new UserData();

        if(GameObject.FindGameObjectWithTag("Player") != null) // If it can find the player
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Set player to the object that has "Player" as its tag
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }




    public void UpgradeBulletSpeed()
    {
        if (player != null) // If player exists
        {
            player.GetComponent<PlayerController>().fireSpeed += 0.1f; // Upgrade the players shooting speed
            m_speedUpgrades++; // Increment the ammount of speed upgrades the ship has
        }
    }

    public void UpgradeBulletPower()
    {
        if (player != null) // If player exists
        {
            player.GetComponent<PlayerController>().firePower += 1; // Upgrade the players shooting power
            m_powerUpgrades++; // Increment the ammount of power upgrades the ship has
        }
    }
}

