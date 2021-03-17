using UnityEngine;

public class UserData
{
    // container that will be serialised and written to disk
    protected class DiskSaveData
    {
        public System.Int64 m_userId = 0;
        public bool m_controlGroup = false;
    }

    public System.Int64 userId
    { get { return m_data.m_userId; } }

    public bool controlGroup
    { get { return m_data.m_controlGroup; } }

    private DiskSaveData m_data = null;
    private FileLoader<DiskSaveData> m_file = new FileLoader<DiskSaveData>("/savedata/userdata.sbl");

    // read data from disk if avalible
    // otherwise generate new data
    public UserData()
    {
        if (m_file.FileExists())
        {
            if (!ReadFromDisk())
            {
                Debug.Log("failed to load data from disk");
            }
        }
        else
        {
            Debug.Log("cound not locate user data, generating new data");
            GenerateNewData();
        }
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
        // generate a sudo random user id
        m_data.m_userId = (System.Int64)Random.Range(System.Int64.MinValue, System.Int64.MaxValue);

        // decide if user is in the control or test group
        if (m_data.m_userId % 2 == 0)
        {
            m_data.m_controlGroup = true;
        }
        return WriteToDisk();
    }
}