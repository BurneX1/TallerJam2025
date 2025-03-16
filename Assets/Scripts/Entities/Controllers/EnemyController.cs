using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Life playerLife;
    public Life life;
    public Shield shield;
    public IncomeRuneChecker incomeRuneChecker;
    public RuneData[] avaiableRunes;

    [Space(5)]
    [Range(0, 3)]
    public int startShields;
    [Range(1,4)]
    public int runesNumber;

    private void Awake()
    {
        if(life == null && GetComponent<Life>() != null) life = GetComponent<Life>();
        if (shield == null && GetComponent<Shield>() != null) shield = GetComponent<Shield>();
        if (incomeRuneChecker == null && GetComponent<IncomeRuneChecker>() != null) incomeRuneChecker = GetComponent<IncomeRuneChecker>();
    }

    private void OnEnable()
    {
        BuildRunes();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (CombatManager.instance.player != null) playerLife = CombatManager.instance.player.GetComponent<Life>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerRemoteDamage(int value)
    {
        if(playerLife!=null) playerLife.LooseHealth(value);
            
    }

    public void BuildRunes()
    {
        if(life!=null)
        {
            life.maxHealth = runesNumber * 1;
            life.currentHealth = runesNumber * 1; //Modificar este numero para settear la vida
        }

        if(avaiableRunes.Length > 0)
        {
            if (shield != null)
            {
                shield.BreakShield();
                for(int i = 0; i < startShields; i++)
                {
                    shield.AddShieldRune(avaiableRunes[Random.Range(0, avaiableRunes.Length)]);
                }
                
            }

            if(incomeRuneChecker!=null)
            {
                incomeRuneChecker.vulnerableRunes.Clear();
                for (int i = 0; i < runesNumber; i++)
                {
                    incomeRuneChecker.AddVulnerabilityByType(avaiableRunes[Random.Range(0, avaiableRunes.Length)]);
                }
            }
        }

    }


}
