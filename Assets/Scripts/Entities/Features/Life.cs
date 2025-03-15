using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Life : MonoBehaviour
{
    public event Action<int> HealthUpdate = delegate {};
    public event Action Dead = delegate {};

    public int maxHealth = 100;

    public int currentHealth;

    public void GainHealth(int amount)
    {
        amount = (int)MathF.Abs(amount);
        if (currentHealth + amount >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
        HealthUpdate.Invoke(amount);
    }

    public void LooseHealth(int amount)
    {
        amount = (int)MathF.Abs(amount);
        if (currentHealth - amount <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }
        HealthUpdate.Invoke(-amount);
    }

    public float GetHealthPercentage()
    {
        return (100 * currentHealth) / maxHealth;
    }

    public void SetHealthPercentage(float percentage)
    {
        
        int newHealth = (int)(maxHealth * percentage) / 100;

        int changeAmout = newHealth - currentHealth;
        HealthUpdate.Invoke(changeAmout);
    }



    //DeadSubscription
    public void OnEnable()
    {
        currentHealth = maxHealth;

        HealthUpdate += (int amount) =>
        { 
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Dead.Invoke();
            }      
        };

        HealthUpdate += (int amount) => Debug.Log(gameObject.name + amount + " life: " + currentHealth);
    }

    public void OnDisable()
    {
        HealthUpdate -= (int amount) =>
        {
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Dead.Invoke();
            }
        };

        HealthUpdate -= (int amount) => Debug.Log(gameObject.name + amount + " life: " + currentHealth);
    }
}
