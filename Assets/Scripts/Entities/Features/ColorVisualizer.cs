using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

public class ColorVisualizer : MonoBehaviour
{

    public IncomeRuneChecker checker;

    public LigthColorGroup[] runeColors;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<IncomeRuneChecker>()) checker = GetComponent<IncomeRuneChecker>();
        SetColors();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (checker) checker.OnRuneDamage += (RuneType type, int dmg) => { RevokeColors(type); };
        SetColors();
    }

    private void OnDisable()
    {
        if (checker) checker.OnRuneDamage -= (RuneType type, int dmg) => { RevokeColors(type); };
    }
    public void SetColors()
    {
        ClearRunes();

        if (checker)
        {
            int i = 0;

            foreach(RuneData data in checker.vulnerableRunes)
            {
                runeColors[i].rune = data;
                i++;
            }
        }

        RefreshColors();
    }

    public void RevokeColors(RuneType type)
    {
        foreach (LigthColorGroup ligths in runeColors)
        {
            if (ligths.rune.RuneType == type)
            {
                ligths.rune = null;
                RefreshColors();
                return;
            }
        }

    }

    public void RefreshColors()
    {
        foreach(LigthColorGroup ligths in runeColors)
        {
            if(ligths.rune == null)
            {
                for(int i = 0; i < ligths.lights.Length; i++)
                {
                    ligths.lights[i].color= new Vector4(0,0,0,0);
                }
            }
            else
            {
                for (int i = 0; i < ligths.lights.Length; i++)
                {
                    ligths.lights[i].color = ligths.rune.RuneColor;

                }
            }
        }

    }

    public void ClearRunes()
    {
        foreach (LigthColorGroup ligths in runeColors)
        {
            ligths.rune = null;
        }
    }
}
[System.Serializable]
public class LigthColorGroup
{
    public RuneData rune;
    public Light2D[] lights;
}