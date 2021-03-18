using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FileLoader<T> where T : class
{
    private string m_path = null;

    // takes full file path from root directory
    public FileLoader(string in_path)
    {
        m_path = in_path;
        Debug.Log("creating file handle [" + in_path + "]");
    }

    public bool FileExists()
    {
        if (File.Exists(m_path))
            return true;
        else
            return false;
    }

    public bool ReadData(out T out_data)
    {
        if (FileExists())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(m_path, FileMode.Open);

            out_data = formatter.Deserialize(stream) as T;
            stream.Close();
        }
        else
        {
            out_data = null;
            return false;
        }
        return true;
    }

    public bool WriteData(T in_data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(m_path, FileMode.Create);
        formatter.Serialize(stream, in_data);
        stream.Close();
        return true;
    }

    public bool DeleteFile()
    {
        return true; // TODO
    }

    public bool CreateDirectory(string dir)
    {
        if (!System.IO.Directory.Exists(dir))
        {
            System.IO.Directory.CreateDirectory(dir);
        }
        return true; // TODO - check return value of System.IO.Directory.CreateDirectory(dir)
    }
}