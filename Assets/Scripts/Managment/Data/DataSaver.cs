using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
[CreateAssetMenu(fileName = "New DataSaver", menuName = "Data/01-Data Saver")]
public class DataSaver : ScriptableObject
{
    public PersistentScriptableObject[] dataObj;
    public void SaveData()
    {
        if (dataObj.Length <= 0) return;
        for (int i = 0; i < dataObj.Length; i++)
        {
            if (dataObj[i] != null)
            {
                dataObj[i].Save();
            }
        }
        ReadmeFile();
    }

    public void LoadData()
    {
        if (dataObj.Length <= 0) return;
        for (int i = 0; i < dataObj.Length; i++)
        {
            if (dataObj[i] != null)
            {
                dataObj[i].Load();
            }
        }
    }

    public void ResetData()
    {
        if (dataObj.Length <= 0) return;
        for (int i = 0; i < dataObj.Length; i++)
        {
            if (dataObj[i] != null)
            {
                dataObj[i].Reset();
            }
        }
    }

    private void ReadmeFile()
    {
        string directory = Path.Combine(Application.persistentDataPath, "BaseGame");

        // Asegúrate de que el directorio exista
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = Path.Combine(directory, "README.txt");
        if (!File.Exists(path))
        {
            string message = "If you edited it, you broke it.";
            File.WriteAllText(path, message);
        }

    }
}
