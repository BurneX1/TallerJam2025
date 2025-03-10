using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public abstract class PersistentScriptableObject : ScriptableObject
{
    public virtual void Save()
    {

        //Save System
        BinaryFormatter formatter = new BinaryFormatter();

        string directory = Path.Combine(Application.persistentDataPath, "Steam");

        Directory.CreateDirectory(directory);

        string path = directory + "/"+ this.name + "GD.dat";
        Debug.Log(path);
        FileStream file = File.Create(path);

        string tmpJson = JsonConvert.SerializeObject(this);
        Debug.Log(tmpJson);

        formatter.Serialize(file, Encrypter.EncryptDecrypt(tmpJson));

        file.Close();
    }
    public virtual void Load()
    {
        //Save System
        string directory = Path.Combine(Application.persistentDataPath, "Steam");

        string path = directory + "/" + this.name + "GD.dat";

        if (!File.Exists(path))
        {
            Save();
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream file = File.Open(path, FileMode.Open);

        string tmpJson = Encrypter.EncryptDecrypt((string)formatter.Deserialize(file));
        Debug.Log(tmpJson);

        JsonUtility.FromJsonOverwrite(tmpJson, this);

        /*object obj = formatter.Deserialize(file);
         = obj;*/

        file.Close();

        Debug.Log("Settings Loaded");
    }

    public virtual void Reset()
    {
        Save();
    }

}
