using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shield : MonoBehaviour
{
    [Header("Configuración de Escudo")]
    [SerializeField] private List<RuneData> shieldRunes = new();
    [SerializeField] private bool isActive = true;

    public event Action<RuneType> OnShieldAbsorbRune = delegate { };
    public event Action OnShieldBroken = delegate { };

    /// <summary>
    /// Comprueba si este escudo bloquea el tipo de runa especificado
    /// </summary>
    /// <param name="runeType">El tipo de runa a comprobar</param>
    /// <returns>Verdadero si el escudo bloquea este tipo de runa</returns>
    public bool BlocksRuneType(RuneType runeType)
    {
        if (!isActive)
            return false;

        foreach (RuneData rune in shieldRunes)
        {
            if (rune.RuneType == runeType)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Procesa una runa absorbida y la elimina del escudo
    /// </summary>
    /// <param name="runeType">El tipo de runa que fue absorbida</param>
    public void AbsorbRune(RuneType runeType)
    {
        if (!isActive)
            return;

        // Encontrar y eliminar la runa coincidente
        for (int i = 0; i < shieldRunes.Count; i++)
        {
            if (shieldRunes[i].RuneType == runeType)
            {
                RuneData removedRune = shieldRunes[i];
                shieldRunes.RemoveAt(i);
                OnShieldAbsorbRune.Invoke(runeType);

                // Comprobar si el escudo está ahora vacío
                if (shieldRunes.Count == 0)
                {
                    BreakShield();
                }

                return;
            }
        }
    }

    /// <summary>
    /// Rompe el escudo, haciendo vulnerable al enemigo
    /// </summary>
    public void BreakShield()
    {
        isActive = false;
        shieldRunes.Clear();
        OnShieldBroken.Invoke();
        Debug.Log($"Escudo roto en {gameObject.name}");
    }

    /// <summary>
    /// Devuelve si la entidad puede recibir daño (sin escudo activo)
    /// </summary>
    public bool CanTakeDamage()
    {
        return !isActive || shieldRunes.Count == 0;
    }

    /// <summary>
    /// Añade una runa al escudo
    /// </summary>
    public void AddShieldRune(RuneData runeData)
    {
        if (!shieldRunes.Contains(runeData))
        {
            shieldRunes.Add(runeData);
            isActive = true;
        }
    }

    /// <summary>
    /// Obtiene las runas actuales del escudo
    /// </summary>
    public List<RuneData> GetShieldRunes()
    {
        return new List<RuneData>(shieldRunes);
    }
}