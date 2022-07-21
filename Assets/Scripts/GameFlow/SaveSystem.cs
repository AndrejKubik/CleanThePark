using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/CurrentSave.txt";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveGameData data = new SaveGameData();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveGameData LoadGame()
    {
        string path = Application.persistentDataPath + "/CurrentSave.txt";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveGameData data = formatter.Deserialize(stream) as SaveGameData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("save not found");
            return null;
        }
    }
}
