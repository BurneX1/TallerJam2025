using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeAction : MonoBehaviour
{
    public float defaultTime;
    [ReadOnly]
    public float timer;
    public bool desableTimer = false;
    private bool endTiming = false;

    [Space(7)]
    //Unity Drag Events
    [SerializeField]
    private UnityEvent TimeOverEvents; 
    [SerializeField]
    private UnityEvent RebuildTimeEvents;


    //Scripted referenced actions
    public event Action EndActions = delegate { };
    public event Action RebuildActions = delegate { };

    // Update is called once per frame
    void Update()
    {
        CountDown();
    }
    public void CountDown()
    {
        if (desableTimer == true) return;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(endTiming==false)
        {
            timer = 0;
            endTiming = true;
            EndActions.Invoke();
        }
    }

    public void RebuildTimer()
    {
        timer = defaultTime;
        endTiming = false;
        RebuildActions.Invoke();
    }

    private void OnEnable()
    {
        RebuildTimer();

        EndActions += () => TimeOverEvents.Invoke();
        RebuildActions += () => RebuildTimeEvents.Invoke();
    }

    private void OnDisable()
    {
        EndActions -= () => TimeOverEvents.Invoke();
        RebuildActions += () => RebuildTimeEvents.Invoke();
    }

}
