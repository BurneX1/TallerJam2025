using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieSpawner : MonoBehaviour
{

    public SpawnPoolManager poolManager;
    public float delaySpawn;
    public string[] avaibleEnemies;

    public Dictionary<Vector2,GameObject> currentEnemies = new Dictionary<Vector2,GameObject>();

    public SpawnSlot[] pointSlots;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < pointSlots.Length; i++)
        {
            StartCoroutine(RegisterObjectToPoint(i, false));
        }
        StartCoroutine(RegisterObjectToPoint(1, false));
        StartCoroutine(RegisterObjectToPoint(1, false));
        StartCoroutine(RegisterObjectToPoint(1, false));
        StartCoroutine(RegisterObjectToPoint(1, false));
        StartCoroutine(RegisterObjectToPoint(1, false));
        StartCoroutine(RegisterObjectToPoint(1, false));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator RegisterObjectToPoint(int slotValue, bool delay)
    {
        if(delay)yield return new WaitForSeconds(delaySpawn);

        if (slotValue >= pointSlots.Length) yield break;
        SpawnSlot slot = pointSlots[slotValue];

        if(slot.spawnPoint==null || pointSlots[slotValue].objSlot != null)
        {
            yield break;
           
        }
        
        GameObject instancedObject = poolManager.Generate(poolManager.GetObjectListValue(avaibleEnemies[Random.Range(0, avaibleEnemies.Length)]), slot.spawnPoint.transform.position,slot.spawnPoint);
        slot.objSlot = instancedObject;
        InstantiatedElement element = instancedObject.GetComponent<InstantiatedElement>();

        currentEnemies.Add(
            new Vector2
            (element.listNumID,
            slotValue)
            , instancedObject
            );

        element.EnableCall += (bool enable, int id) => 
        {
            if (enable == false) UnRegisterObject(slotValue,id);
        };

        if (CombatManager.instance) CombatManager.instance.AddEnemy(instancedObject);
        //Insertar función para agregar elementos al combat Manager

    }

    public void UnRegisterObject(int slotValue, int objectID)
    {
        if (slotValue >= pointSlots.Length) return;
        SpawnSlot slot = pointSlots[slotValue];

        currentEnemies.Remove(new Vector2(objectID, slotValue));

        InstantiatedElement element = slot.objSlot.GetComponent<InstantiatedElement>();

        //Insertar función para quitar elementos al combat Manager
        if (CombatManager.instance) CombatManager.instance.RemoveEnemy(slot.objSlot);

        slot.objSlot = null;

        
        element.EnableCall -= (bool enable, int id) =>
        {
            if (enable == false) UnRegisterObject(slotValue, id);
        };

        
        

        StartCoroutine(RegisterObjectToPoint(slotValue, true));
    }


}
[System.Serializable]
public class SpawnSlot
{
    public GameObject objSlot;
    public Transform spawnPoint;
}
