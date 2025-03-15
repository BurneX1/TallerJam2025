using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeRuneChecker : MonoBehaviour
{
    [Header("Configuración de Vulnerabilidad")]
    [SerializeField] private List<RuneData> vulnerableRunes = new();
    [SerializeField] private Life enemyLife;
    [SerializeField] private int defaultDamage = 10; // Daño por defecto si la runa no tiene valor de daño

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
                ApplyRuneDamage(runeType);
                Debug.Log($"Enemigo {gameObject.name} recibió daño de la runa {runeType}");
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

    private void ApplyRuneDamage(RuneType runeType)
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
        }
    }

    // Método para añadir una nueva vulnerabilidad
    public void AddVulnerability(RuneData runeData)
    {
        if (!vulnerableRunes.Contains(runeData))
        {
            vulnerableRunes.Add(runeData);
        }
    }

    // Método para eliminar una vulnerabilidad
    public void RemoveVulnerability(RuneData runeData)
    {
        if (vulnerableRunes.Contains(runeData))
        {
            vulnerableRunes.Remove(runeData);
        }
    }
}