using UnityEngine;

public class UserData
{
    // container that will be serialized and written to disk
    [System.Serializable]
    protected class DiskUserData
    {
        public System.Int64 m_userId = 0;
        public bool m_controlGroup = false;
        public float m_masterVolume = 1f;
        public float m_musicVolume = 1f;
        public float m_sfxVolume = 1f;
    }

    [System.Serializable]
    protected class DiskGameData
    {
        public int m_money = 0;
        public int m_speedUpgrades = 0; // Variable to display to player how many times firing speed has been upgraded
        public int m_powerUpgrades = 0; // Variable to display to player how many times firing power has been upgraded
    }

    private DiskUserData m_userData = null;
    private DiskGameData m_gameData = null;

    private FileLoader<DiskUserData> m_userFile;
    private FileLoader<DiskGameData> m_gameFile;

    public System.Int64 userId
    { get { return m_userData.m_userId; } }

    public bool controlGroup
    {
        get { return m_userData.m_controlGroup; }
        set { m_userData.m_controlGroup = value; }
    }

    public int money
    {
        get { return m_gameData.m_money; }
        set { m_gameData.m_money = value; }
    }

    public int speedUpgrade
    {
        get { return m_gameData.m_speedUpgrades; }
        set { m_gameData.m_speedUpgrades = value; }
    }

    public int powerUpgrade
    {
        get { return m_gameData.m_powerUpgrades; }
        set { m_gameData.m_powerUpgrades = value; }
    }

    public float masterVolume
    {
        get { return m_userData.m_masterVolume; }
        set { m_userData.m_masterVolume = value; }
    }

    public float musicVolume
    {
        get { return m_userData.m_musicVolume; }
        set { m_userData.m_musicVolume = value; }
    }

    public float sfxVolume
    {
        get { return m_userData.m_sfxVolume; }
        set { m_userData.m_sfxVolume = value; }
    }

    // read data from disk if available
    // otherwise generate new data
    public UserData()
    {
        m_userFile = new FileLoader<DiskUserData>(GameController.Instance.applicationPath + "/savedata/userdata.sbd");
        m_gameFile = new FileLoader<DiskGameData>(GameController.Instance.applicationPath + "/savedata/gamedata.sbg");

        m_userFile.CreateDirectory(GameController.Instance.applicationPath + "/savedata/");
        if (m_userFile.FileExists())
        {
            if (!m_userFile.ReadData(out m_userData))
            {
                Debug.Log("failed to read userdata from disk");
            }
        }
        else
        {
            GenerateNewData();
        }

        if (m_gameFile.FileExists())
        {
            if (!m_gameFile.ReadData(out m_gameData))
            {
                Debug.Log("failed to read gamedata from disk");
            }
        }
        else
        {
            m_gameData = new DiskGameData();
        }

        // write user id to console in hexadecimal
        Debug.Log("user_id = " + m_userData.m_userId.ToString("X"));
    }

    //private bool ReadFromDisk()
    //{
    //    return m_userFile.ReadData(out m_userData);
    //}

    public bool WriteToDisk()
    {
        bool r1 = m_userFile.WriteData(m_userData);
        bool r2 = m_gameFile.WriteData(m_gameData);
        return r1 && r2;
    }

    private bool GenerateNewData()
    {
        m_userData = new DiskUserData();

        // generate a pseudo random user id
        System.Int64 r1 = Random.Range(System.Int32.MinValue, System.Int32.MaxValue);
        System.Int64 r2 = Random.Range(System.Int32.MinValue, System.Int32.MaxValue);
        m_userData.m_userId = (r1 << 32) | r2;

        // decide if user is in the control or test group
        if (m_userData.m_userId % 2 == 0)
        {
            m_userData.m_controlGroup = true;
        }
        return m_userFile.WriteData(m_userData);
    }

    public void ClearAllData(bool deleteUserId = false)
    {
        m_userFile.DestroyDirectory(GameController.Instance.applicationPath + "/savedata/leveldata/");
        m_gameFile.DeleteFile();
        m_gameData = new DiskGameData();

        if (deleteUserId)
        {
            m_userFile.DeleteFile();
            m_userData = null;
            GenerateNewData();
        }
    }
}