using UnityEngine;

public class UserData
{
    // TODO - getters/setters

    // container that will be serialised and written to disk
    protected class DiskSaveData
    {
        public System.Int64 m_userId = 0;
        public bool m_controlGroup = false;
    }

    private DiskSaveData m_data = null;
    private FileLoader<DiskSaveData> m_file = new FileLoader<DiskSaveData>("/savedata/userdata.sbl");

    // read data from disk if avalible
    // otherwise generate new data
    private void initialise()
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
        m_file.ReadData(out m_data);

        // TODO - transfer data out of saveData

        return true;
    }

    private bool WriteToDisk()
    {
        // TODO - file write
        return true;
    }

    private bool GenerateNewData()
    {
        System.Random rnd = new System.Random();

        // user id

        System.Int32 r1 = rnd.Next();
        System.Int32 r2 = rnd.Next();
        m_data.m_userId = (r1 << 32) | r2;// TODO - ensure no duplicate random numbers are generated

        // decide if user is in the control or test group
        if (m_data.m_userId % 2 == 0)
        {
            m_data.m_controlGroup = true;
        }

        return WriteToDisk();
    }
}