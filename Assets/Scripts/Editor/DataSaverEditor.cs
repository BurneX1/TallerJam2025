using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DataSaver))]
public class DataSaverEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DataSaver dataSv = (DataSaver)target;
        if (GUILayout.Button("Save"))
        {
            dataSv.SaveData();
        }
        if (GUILayout.Button("Load"))
        {
            dataSv.LoadData();
        }
        if (GUILayout.Button("Reset"))
        {
            dataSv.ResetData();
        }
    }
}
