using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Life life;
    public IncomeRuneChecker incomeRuneChecker;
    public RuneData[] avaiableRunes;
    [Space(5)]
    [Range(0, 3)]
    public int startShields;
    [Range(1,4)]
    public int runesNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
