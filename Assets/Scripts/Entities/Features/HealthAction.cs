using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthAction : MonoBehaviour
{
    public Life life;

    public HealthEvent[] AttachablkeEvents;

    private void Awake()
    {
        if (life == null && gameObject.GetComponent<Life>()) life = gameObject.GetComponent<Life>();
    }

    public void CheckActions()
    {
        if (life == null) return;
        if(AttachablkeEvents.Length>0)
        {
            for(int i = 0; i < AttachablkeEvents.Length; i++)
            {
                if (AttachablkeEvents[i].invoked==false && life.GetHealthPercentage() <= AttachablkeEvents[i].healthPercentage)
                {
                    if (AttachablkeEvents[i].ignoreDeath==false&&life.currentHealth<=0) continue;

                    AttachablkeEvents[i].EvokeAction.Invoke();
                    AttachablkeEvents[i].invoked = true;
                }
            }
        }
    }

    public void RebuildActions()
    {
        for (int i = 0; i < AttachablkeEvents.Length; i++)
        {
            AttachablkeEvents[i].invoked = false;
        }
    }

    private void OnEnable()
    {
        RebuildActions();
        if (life != null) life.HealthUpdate += (int value) => CheckActions();
    }

    private void OnDisable()
    {
        if (life != null) life.HealthUpdate -= (int value) => CheckActions();
    }
}
[System.Serializable]
public class HealthEvent
{
    [Range(0f, 100f)]
    public float healthPercentage;
    public UnityEvent EvokeAction;
    public bool ignoreDeath;
    public bool invoked;
}
