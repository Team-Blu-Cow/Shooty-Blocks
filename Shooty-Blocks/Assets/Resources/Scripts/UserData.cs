using UnityEngine;

public class UserData
{
    // container that will be serialised and written to disk
    [System.Serializable]
    protected class DiskUserData
    {
        public System.Int64 m_userId = 0;
        public bool m_controlGroup = false;
    }

    private DiskUserData m_data = null;
    private FileLoader<DiskUserData> m_file;// = new FileLoader<DiskSaveData>("/savedata/userdata.sbl");

    public System.Int64 userId
    { get { return m_data.m_userId; } }

    public bool controlGroup
    { get { return m_data.m_controlGroup; } }

    // read data from disk if avalible
    // otherwise generate new data
    public UserData()
    {
        m_file = new FileLoader<DiskUserData>(GameController.Instance.applicationPath + "/savedata/userdata.sbd");
        m_file.CreateDirectory(GameController.Instance.applicationPath + "/savedata/");

        if (m_file.FileExists())
        {
            if (!ReadFromDisk())
            {
                Debug.Log("failed to read userdata from disk");
                return;
            }
        }
        else
        {
            GenerateNewData();
        }

        // write user id to console in hexadecimal
        Debug.Log("user_id = " + m_data.m_userId.ToString("X"));
    }

    private bool ReadFromDisk()
    {
        return m_file.ReadData(out m_data);
    }

    public bool WriteToDisk()
    {
        return m_file.WriteData(m_data);
    }

    private bool GenerateNewData()
    {
        m_data = new DiskUserData();

        // generate a sudo random user id
        System.Random rnd = new System.Random();
        System.Int64 r1 = rnd.Next(System.Int32.MinValue, System.Int32.MaxValue);
        System.Int64 r2 = rnd.Next(System.Int32.MinValue, System.Int32.MaxValue);
        m_data.m_userId = (r1 << 32) | r2;

        // decide if user is in the control or test group
        if (m_data.m_userId % 2 == 0)
        {
            m_data.m_controlGroup = true;
        }
        return WriteToDisk();
    }
}