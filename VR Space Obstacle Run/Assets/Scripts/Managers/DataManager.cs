using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

public class DataManager : MonoBehaviour {

    public static DataManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public enum SerializationMode 
    {
        Binary,
        //JSON,
        XML,
    }

    /// <summary>
    /// Use this function to save a single file inheriting from SerializableData.
    /// </summary>
    public void SaveFile(SerializableData toSave, string directory, string fileName, SerializationMode saveMode)
    {
        CheckDirectory(directory, true);

        switch (saveMode)
        {
            case SerializationMode.Binary:
                SaveByBinary(new SerializableData[] { toSave }, Path.Combine(directory, fileName));
                break;
            case SerializationMode.XML:
                SaveByXML(new SerializableData[] { toSave }, Path.Combine(directory, fileName));
                break;
        }

        Debug.Log("File saved to " + directory);
    }

    /// <summary>
    /// Use this function to save an array of files inheriting from SerializableData.
    /// </summary>
    public void SaveFiles(SerializableData[] toSave, string directory, string fileName, SerializationMode saveMode)
    {
        CheckDirectory(directory, true);

        switch (saveMode)
        {
            case SerializationMode.Binary:
                SaveByBinary(toSave, Path.Combine(directory, fileName));
                break;
            case SerializationMode.XML:
                SaveByXML(toSave, Path.Combine(directory, fileName));
                break;
        }

        Debug.Log("Files saved to " + directory);
    }

    /// <summary>
    /// Use this function to load a single file.
    /// </summary>
    public SerializableData LoadFile(string directory, string fileName, SerializationMode loadMode)
    {
        if (File.Exists(Path.Combine(directory, fileName)))
        {
            SerializableData toReturn = null;

            switch (loadMode)
            {
                case SerializationMode.Binary:
                    toReturn = LoadByBinary(Path.Combine(directory, fileName))[0];
                    break;
                case SerializationMode.XML:
                    toReturn = LoadByJSON(Path.Combine(directory, fileName))[0];
                    break;
            }

            return toReturn;
        }

        Debug.LogError("The File you are trying to load does not exist. File name " + fileName);
        return null;
    }

    /// <summary>
    /// Use this function to load an array of files.
    /// </summary>
    public SerializableData[] LoadFiles(string directory, string fileName, SerializationMode loadMode)
    {
        if (File.Exists(Path.Combine(directory, fileName)))
        {
            SerializableData[] toReturn = null;

            switch (loadMode)
            {
                case SerializationMode.Binary:
                    toReturn = LoadByBinary(Path.Combine(directory, fileName));
                    break;
                case SerializationMode.XML:
                    toReturn = LoadByJSON(Path.Combine(directory, fileName));
                    break;
            }

            return toReturn;
        }

        Debug.LogError("The File you are trying to load does not exist. File name " + fileName);
        return null;
    }

    /// <summary>
    /// Will save files with the binary formatter to a given path.
    /// </summary>
    private void SaveByBinary(SerializableData[] toSave, string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Create(path);

        bf.Serialize(fileStream, toSave);
        fileStream.Close();
    }

    /// <summary>
    ///  Will save files to a JSON file to a given path.
    /// </summary>
    private void SaveByJSON(SerializableData[] toSave, string path)
    {
        string json = string.Empty;
        List<string> save = new List<string>();

        for (int i = 0; i < toSave.Length; i++)
        {
            string temp = JsonUtility.ToJson(toSave[i], true);
            json += temp;
        }

        StreamWriter sw = File.CreateText(path);
        sw.Close();

        File.WriteAllText(path, json);
    }

    private void SaveByXML(SerializableData[] toSave, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SerializableData[]));
        FileStream fileStream = File.Create(path);
        serializer.Serialize(fileStream, toSave);
        fileStream.Close();
    }

    /// <summary>
    /// Loads binary files from a given path
    /// </summary>
    private SerializableData[] LoadByBinary(string path)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fileStream = File.Open(path, FileMode.Open);

        SerializableData[] toReturn = (SerializableData[])bf.Deserialize(fileStream);
        fileStream.Close();

        return toReturn;
    }

    /// <summary>
    /// Loads JSON files from a given path
    /// </summary>
    private SerializableData[] LoadByJSON(string path)
    {
        string json = File.ReadAllText(path);
        SerializableData[] toReturn = JsonUtility.FromJson<SerializableData[]>(json);

        return toReturn;
    }

    /// <summary>
    /// Checks the given directory if it exists and makes creates it if it doesn't.
    /// </summary>
    private bool CheckDirectory(string directory, bool force)
    {
        if (!Directory.Exists(directory))
        {
            if (force)
            {
                Debug.LogWarning("The directory " + directory + " did not exist, but has now been created.");
                Directory.CreateDirectory(directory);
                return true;
            }
            else
            {
                Debug.LogError("The directory " + directory + " Does not exist.");
                return false;
            }
        }

        return false;
    }
}
