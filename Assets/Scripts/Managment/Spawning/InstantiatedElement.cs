using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InstantiatedElement : MonoBehaviour
{
    public SpawnPoolManager spawner;
    public int listNumID;

    public event Action<bool,int> EnableCall = delegate { };

    private void OnEnable()
    {
        if (spawner) spawner.MoveElementInArrayTo(listNumID, true,gameObject);
        EnableCall.Invoke(true,listNumID);
    }

    private void OnDisable()
    {
        EnableCall.Invoke(false, listNumID);
        if (spawner) spawner.MoveElementInArrayTo(listNumID, false, gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
