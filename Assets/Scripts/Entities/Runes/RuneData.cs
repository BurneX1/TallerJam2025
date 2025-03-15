using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuneData", menuName = "Runes/RuneData", order = 1)]
public class RuneData : ScriptableObject
{
    [Header("Propiedades de la Runa")]
    [SerializeField] private RuneType runeType;
    [SerializeField] private int damageValue = 10; // Valor predeterminado
    [SerializeField] private int runeValue;

    public RuneType RuneType => runeType;
    public int RuneValue => runeValue;
    public int DamageValue => damageValue; // Propiedad pública para acceder al valor de daño

    public void SetupRune(RuneType type)
    {
        runeType = type;
        runeValue = (int)type;
        // Puedes asignar un daño por defecto según el tipo si lo deseas
        // Por ejemplo, runas más avanzadas podrían hacer más daño
        damageValue = 10 + ((int)type * 5); // Solo un ejemplo
    }

    public int GetRuneTypeAsInt()
    {
        return (int)runeType;
    }
}