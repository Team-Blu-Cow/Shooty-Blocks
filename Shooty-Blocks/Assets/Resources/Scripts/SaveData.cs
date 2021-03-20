using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    [System.Serializable]
    private class DiskSaveData
    {
        // 64th bit in coinData stores whether or not the level has
        // been completed
        public System.Int64 m_coinData = 0;
    }

    private FileLoader<DiskSaveData> m_file;
    private DiskSaveData m_data;

    // in_levelName - name of level, is used as a filename on disk
    // out_wasInitialised - if this level has save data on disk
    public SaveData(string in_levelName, out bool out_wasInitialised)
    {
        m_file = new FileLoader<DiskSaveData>(GameController.Instance.applicationPath + "/savedata/leveldata/" + in_levelName + ".sbl");
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

    // returns if a given coin has been collected
    // expected values [0 - 62]
    public bool IsCoinCollected(int coin)
    {
        System.Int64 one = 1;
        return (m_data.m_coinData & (one << coin)) != 0;
    }

    // sets if a given coin has been collected
    // expected values [0 - 62]
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

    // return state of 64th bit in coinData
    public bool IsLevelComplete()
    {
        System.Int64 one = 1;
        return (m_data.m_coinData & one) != 0;
    }

    // set state of 64th bit in coinData
    public void SetLevelComplete(bool complete)
    {
        System.Int64 one = 1;
        System.Int64 mask = m_data.m_coinData | (one << 63);

        if (complete)
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
        m_file.CreateDirectory(GameController.Instance.applicationPath + "/savedata/leveldata/");
        return m_file.WriteData(m_data);
    }
}