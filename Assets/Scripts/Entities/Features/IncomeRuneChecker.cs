using System;
using System.Collections.Generic;
using UnityEngine;

public class IncomeRuneChecker : MonoBehaviour
{
    [Header("Configuraci�n de Vulnerabilidad")]
    public List<RuneData> vulnerableRunes = new();
    [SerializeField] private Life enemyLife;
    [SerializeField] private int defaultDamage = 10; // Da�o por defecto si la runa no tiene valor de da�o

    // Evento que se dispara cuando una runa hace da�o exitosamente
    public event Action<RuneType, int> OnRuneDamage; // Tipo de runa y da�o aplicado

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
                return false; // Runa destruida, no contin�a
            }
        }

        // Si no hay escudo o el escudo no bloquea, verificar vulnerabilidad
        if (IsVulnerableTo(runeType))
        {
            if (shield == null || shield.CanTakeDamage()) // Solo da�a si no hay escudo o el escudo permite da�o
            {
                int damageApplied = ApplyRuneDamage(runeType);
                Debug.Log($"Enemigo {gameObject.name} recibi� da�o de la runa {runeType}");

                // Invocar el evento con el tipo de runa y el da�o aplicado
                OnRuneDamage?.Invoke(runeType, damageApplied);

                // Eliminar la vulnerabilidad a esta runa despu�s de aplicar el da�o
                RemoveVulnerabilityByType(runeType);
            }
            else
            {
                Debug.Log($"Enemigo {gameObject.name} tiene escudo, no puede recibir da�o de la runa {runeType}");
            }
        }
        else
        {
            Debug.Log($"Enemigo {gameObject.name} no es vulnerable a la runa {runeType}");
        }

        // La runa contin�a al siguiente enemigo ya que no fue bloqueada por un escudo
        return true;
    }

    /// <summary>
    /// Aplica el da�o de una runa espec�fica al enemigo
    /// </summary>
    /// <param name="runeType">El tipo de runa a procesar</param>
    /// <returns>La cantidad de da�o aplicado</returns>
    private int ApplyRuneDamage(RuneType runeType)
    {
        if (enemyLife != null)
        {
            // Buscar la runa correspondiente para obtener su valor de da�o espec�fico
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
            Debug.Log($"Aplicando {damageToApply} de da�o al enemigo {gameObject.name} con la runa {runeType}");
            return damageToApply;
        }
        return 0;
    }

    /// <summary>
    /// Elimina la vulnerabilidad a un tipo espec�fico de runa del enemigo cuando se aplica el da�o
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
    /// A�ade una vulnerabilidad a un tipo espec�fico de runa al enemigo,
    /// en caso se quieran a�adir vulnerabilidades de forma din�mica
    /// </summary>
    /// <param name="runeType"></param>
    public void AddVulnerabilityByType(RuneData rune)
    {
        vulnerableRunes.Add(rune);
    }

}