using System;
using System.Collections.Generic;
using UnityEngine;

public class IncomeRuneChecker : MonoBehaviour
{
    [Header("Configuración de Vulnerabilidad")]
    public List<RuneData> vulnerableRunes = new();
    [SerializeField] private Life enemyLife;
    [SerializeField] private int defaultDamage = 10; // Daño por defecto si la runa no tiene valor de daño

    // Evento que se dispara cuando una runa hace daño exitosamente
    public event Action<RuneType, int> OnRuneDamage; // Tipo de runa y daño aplicado

    private Shield shield;

    private void Awake()
    {
        if (enemyLife == null)
            enemyLife = GetComponent<Life>();

        shield = GetComponent<Shield>();
    }

    /// <summary>
    /// Comprueba si este enemigo tiene una vulnerabilidad a la runa especificada
    /// </summary>
    /// <param name="runeType">El tipo de runa a comprobar</param>
    /// <returns>Verdadero si el enemigo es vulnerable a este tipo de runa</returns>
    public bool IsVulnerableTo(RuneType runeType)
    {
        foreach (RuneData rune in vulnerableRunes)
        {
            if (rune.RuneType == runeType)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Procesa una runa entrante y determina si debe continuar al siguiente enemigo
    /// </summary>
    /// <param name="runeType">El tipo de runa a procesar</param>
    /// <returns>Verdadero si la runa debe continuar al siguiente enemigo, falso si se rompe</returns>
    public bool ProcessRune(RuneType runeType)
    {
        // Comprobar si el enemigo tiene un escudo
        if (shield != null)
        {
            // Si el escudo bloquea este tipo de runa, la runa es destruida
            if (shield.BlocksRuneType(runeType))
            {
                shield.AbsorbRune(runeType);
                Debug.Log($"Runa {runeType} absorbida por el escudo en {gameObject.name}");
                return false; // Runa destruida, no continúa
            }
        }

        // Si no hay escudo o el escudo no bloquea, verificar vulnerabilidad
        if (IsVulnerableTo(runeType))
        {
            if (shield == null || shield.CanTakeDamage()) // Solo daña si no hay escudo o el escudo permite daño
            {
                int damageApplied = ApplyRuneDamage(runeType);
                Debug.Log($"Enemigo {gameObject.name} recibió daño de la runa {runeType}");

                // Invocar el evento con el tipo de runa y el daño aplicado
                OnRuneDamage?.Invoke(runeType, damageApplied);

                // Eliminar la vulnerabilidad a esta runa después de aplicar el daño
                RemoveVulnerabilityByType(runeType);
            }
            else
            {
                Debug.Log($"Enemigo {gameObject.name} tiene escudo, no puede recibir daño de la runa {runeType}");
            }
        }
        else
        {
            Debug.Log($"Enemigo {gameObject.name} no es vulnerable a la runa {runeType}");
        }

        // La runa continúa al siguiente enemigo ya que no fue bloqueada por un escudo
        return true;
    }

    /// <summary>
    /// Aplica el daño de una runa específica al enemigo
    /// </summary>
    /// <param name="runeType">El tipo de runa a procesar</param>
    /// <returns>La cantidad de daño aplicado</returns>
    private int ApplyRuneDamage(RuneType runeType)
    {
        if (enemyLife != null)
        {
            // Buscar la runa correspondiente para obtener su valor de daño específico
            int damageToApply = defaultDamage;

            foreach (RuneData rune in vulnerableRunes)
            {
                if (rune.RuneType == runeType)
                {
                    // Si la runa tiene un campo damageValue, usamos ese valor
                    damageToApply = rune.DamageValue > 0 ? rune.DamageValue : defaultDamage;
                    break;
                }
            }

            enemyLife.LooseHealth(damageToApply);
            Debug.Log($"Aplicando {damageToApply} de daño al enemigo {gameObject.name} con la runa {runeType}");
            return damageToApply;
        }
        return 0;
    }

    /// <summary>
    /// Elimina la vulnerabilidad a un tipo específico de runa del enemigo cuando se aplica el daño
    /// </summary>
    /// <param name="runeType">El tipo de runa a eliminar</param>
    private void RemoveVulnerabilityByType(RuneType runeType)
    {
        for (int i = vulnerableRunes.Count - 1; i >= 0; i--)
        {
            if (vulnerableRunes[i].RuneType == runeType)
            {
                Debug.Log($"Eliminando vulnerabilidad a la runa {runeType} del enemigo {gameObject.name}");
                vulnerableRunes.RemoveAt(i);
                break; // Eliminamos solo la primera ocurrencia
            }
        }
    }

    /// <summary>
    /// Añade una vulnerabilidad a un tipo específico de runa al enemigo,
    /// en caso se quieran añadir vulnerabilidades de forma dinámica
    /// </summary>
    /// <param name="runeType"></param>
    public void AddVulnerabilityByType(RuneData rune)
    {
        vulnerableRunes.Add(rune);
    }

}