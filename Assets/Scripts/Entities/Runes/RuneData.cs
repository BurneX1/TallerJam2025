using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneData", menuName = "Runes/RuneData", order = 1)]
public class RuneData : ScriptableObject
{
    [Header("Rune Properties")]
    [SerializeField] private RuneType runeType;
    [SerializeField] private int runeValue;

    public RuneType RuneType => runeType;
    public int RuneValue => runeValue;

    public void SetupRune(RuneType type)
    {
        runeType = type;
        runeValue = (int)type;
    }

    public int GetRuneTypeAsInt()
    {
        return (int)runeType;
    }
}