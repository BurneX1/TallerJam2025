using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SpawnPoolManager : MonoBehaviour
{
    public GeneretableObject[] objectsList;
    public static SpawnPoolManager instance;
    private void Awake()
    {
        instance = this;    
        SetUp();
    }


    public void SetUp()
    {
        for(int i = 0; i < objectsList.Length;i++)
        {
            if(objectsList[i].activeObjects.Length>0)
            {
                for (int a = 0; a < objectsList[i].activeObjects.Length; a++)
                {
                    MoveElementInArrayTo(i, false, objectsList[i].activeObjects[a]);
                }
            }
            for (int e = 0; e < objectsList[i].deactiveObjects.Length;e++)
            {
                InstantiatedElement spawnCmp = objectsList[i].deactiveObjects[e].AddComponent<InstantiatedElement>();
                spawnCmp.spawner = this;
                spawnCmp.listNumID = i;
                objectsList[i].deactiveObjects[e].SetActive(false);
            }
        }
    }

    public int GetObjectListValue(string searchName)
    {
        for(int i =0; i < objectsList.Length;i++)
        {
            if(objectsList[i].nameObj == searchName)
            {
                return i;
            }
        }
        return -1;
    }
    public void FastGenerate(int listValue)
    {
        Generate(listValue);
    }
    public GameObject Generate(int listValue)
    {
        if (listValue < 0) return null;
        if(objectsList[listValue].deactiveObjects.Length>0)
        {
            GameObject actualObj = objectsList[listValue].deactiveObjects[0];
            actualObj.SetActive(true);
            return actualObj;
        }
        else
        {
            GameObject actualObj = Instantiate(objectsList[listValue].mainPrefabObj);
            actualObj.SetActive(false);

            InstantiatedElement spawnCmp = actualObj.AddComponent<InstantiatedElement>();
            spawnCmp.spawner = this;
            spawnCmp.listNumID = listValue;
            objectsList[listValue].deactiveObjects = objectsList[listValue].deactiveObjects.Concat(new GameObject[] { actualObj }).ToArray();
            actualObj.SetActive(true);
            return actualObj;
        }
    }
    public GameObject Generate(int listValue,Vector3 position)
    {
        GameObject obj = Generate(listValue);
        obj.transform.position = position;
        return obj;
    }

    public GameObject Generate(int listValue, Transform parent)
    {
        GameObject obj = Generate(listValue);
        obj.transform.SetParent(parent);
        return obj;
    }

    public GameObject Generate(int listValue, Vector3 position, Transform parent)
    {
        GameObject obj = Generate(listValue);
        obj.transform.position = position;
        obj.transform.SetParent(parent);
        return obj;
    }


    public void MoveElementInArrayTo(int numList, bool activeList, GameObject selfObj)
    {
        if(activeList==true)
        {
            GameObject actualObj = selfObj;
            objectsList[numList].activeObjects = objectsList[numList].activeObjects.Concat(new GameObject[] { actualObj }).ToArray();
            objectsList[numList].deactiveObjects = objectsList[numList].deactiveObjects.Where((e) => e.GetInstanceID() != selfObj.GetInstanceID()).ToArray();

        }
        else
        {
            GameObject actualObj = selfObj;
            objectsList[numList].deactiveObjects = objectsList[numList].deactiveObjects.Concat(new GameObject[] { actualObj }).ToArray();
            objectsList[numList].activeObjects = objectsList[numList].activeObjects.Where((e) => e.GetInstanceID() != selfObj.GetInstanceID()).ToArray();
 
        }

    }
}
[System.Serializable]
public class GeneretableObject
{
    public string nameObj;
    public GameObject mainPrefabObj;
    public GameObject[] activeObjects;
    public GameObject[] deactiveObjects;
}
