using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// format for data the will be saved to disk
public class SaveData
{
    Int64 player_id = 0; // the unique id assigned to the user
    bool control_group = true; // if the user is in the control group or test group
}

public class FileLoading
{
    private static string m_path = Application.persistentDataPath + "/savedata/data.sbl";

    public static string GetFilePath()
    {
        return m_path;
    }

    public static bool SaveDataExists()
    {
        if (File.Exists(GetFilePath()))
            return true;
        else
            return false;
    }

    public static bool ReadGameData(out SaveData out_gameData)
    {
        if (SaveDataExists())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GetFilePath(), FileMode.Open);

            out_gameData = formatter.Deserialize(stream) as SaveData;
            stream.Close();
        }
        else
        {
            out_gameData = new SaveData();
            return false;
        }
        return true;
    }

    public static bool WriteGameData(SaveData in_saveData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(GetFilePath(), FileMode.Create);
        formatter.Serialize(stream, in_saveData);
        stream.Close();
        return true;
    }
}