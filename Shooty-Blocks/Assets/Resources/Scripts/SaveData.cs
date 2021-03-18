using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    [System.Serializable]
    private class DiskSaveData
    {
        public System.Int64 m_coinData = 0;
    }

    private FileLoader<DiskSaveData> m_file;
    private DiskSaveData m_data;

    // in_levelName - name of level, is used as a filename on disk
    // out_wasInitialised - if this level has been previously played
    public SaveData(string in_levelName, out bool out_wasInitialised)
    {
        m_file = new FileLoader<DiskSaveData>(GameController.Instance.applicationPath + in_levelName);
        if (m_file.FileExists())
        {
            out_wasInitialised = true;
            m_file.ReadData(out m_data);
        }
        else
        {
            out_wasInitialised = false;
            m_data = new DiskSaveData();
        }
    }

    // returns if a given coin haes been collected
    // expected values [0 - 63]
    public bool IsCoinCollected(int coin)
    {
        System.Int64 one = 1;
        return (m_data.m_coinData & (one << coin)) != 0;
    }

    // sets if a given coin haes been collected
    // expected values [0 - 63]
    public void SetCoinCollected(int coin, bool collected)
    {
        System.Int64 one = 1;
        System.Int64 mask = m_data.m_coinData | (one << coin);

        if (collected)
        {
            m_data.m_coinData |= mask;
        }
        else
        {
            m_data.m_coinData &= ~mask;
        }
    }

    // saves the contents of m_data to disk
    public bool WriteToDisk()
    {
        return m_file.WriteData(m_data);
    }
}