using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneData", menuName = "Runes/RuneData", order = 1)]
public class RuneData : ScriptableObject
{
    [Header("Propiedades de la Runa")]
    [SerializeField] private RuneType runeType;
    [SerializeField] private int damageValue = 10; // Valor predeterminado
    [SerializeField] private int runeValue;
    [SerializeField] private Color runeColor;

    public RuneType RuneType => runeType; // Propiedad p�blica para acceder al tipo de runa
    public int RuneValue => runeValue; // Propiedad p�blica para acceder al valor de la runa
    public int DamageValue => damageValue; // Propiedad p�blica para acceder al valor de da�o
    public Color RuneColor => runeColor; // Propiedad p�blica para acceder al color de la runa

    public void SetupRune(RuneType type)
    {
        runeType = type;
        runeValue = (int)type;
        // Puedes asignar un da�o por defecto seg�n el tipo si lo deseas
        // Por ejemplo, runas m�s avanzadas podr�an hacer m�s da�o
        damageValue = 10 + ((int)type * 5); // Solo un ejemplo
    }

    public int GetRuneTypeAsInt()
    {
        return (int)runeType;
    }
}