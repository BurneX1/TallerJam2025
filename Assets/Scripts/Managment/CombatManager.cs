using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    [Header("Configuración de Combate")]
    public GameObject player;
    [Space(2.5f)]
    [SerializeField] private List<GameObject> enemies = new();
    [SerializeField] private float delayBetweenRunes = 0.5f;

    private RuneKeyEvoker runeKeyEvoker;

    private bool isCombatActive = false;

    void Awake()
    {
        instance = this;
        // Obtener la instancia de RuneKeyEvoker
        runeKeyEvoker = FindObjectOfType<RuneKeyEvoker>();
        // Suscribirse al evento de secuencia de runas de RuneKeyEvoker
        runeKeyEvoker.OnRuneSequenceSubmitted += ProcessRuneSequence;
    }

    private void OnDestroy()
    {
        // Desuscribirse para evitar pérdidas de memoria
        if (runeKeyEvoker != null)
        {
            runeKeyEvoker.OnRuneSequenceSubmitted -= ProcessRuneSequence;
        }
    }

    /// <summary>
    /// Procesa una secuencia de runas enviada por el jugador
    /// </summary>
    /// <param name="runeSequence">La secuencia de runas a procesar</param>
    public void ProcessRuneSequence(List<RuneType> runeSequence)
    {
        if (isCombatActive)
        {
            Debug.LogWarning("El combate ya está activo, no se puede procesar una nueva secuencia de runas");
            return;
        }

        if (runeSequence.Count == 0)
        {
            Debug.LogWarning("Secuencia de runas vacía, nada que procesar");
            return;
        }

        // Obtener enemigos activos (filtrando los que puedan ser nulos/destruidos)
        enemies = enemies.Where(e => e != null).ToList();

        if (enemies.Count == 0)
        {
            Debug.LogWarning("No hay enemigos a los que atacar");
            return;
        }

        // Iniciar el procesamiento de la secuencia de runas
        StartCoroutine(ProcessRuneSequenceCoroutine(runeSequence));
    }

    /// <summary>
    /// Corrutina para procesar cada runa en la secuencia con un retraso entre ellas
    /// </summary>
    private IEnumerator ProcessRuneSequenceCoroutine(List<RuneType> runeSequence)
    {
        isCombatActive = true;

        foreach (RuneType runeType in runeSequence)
        {
            yield return ProcessSingleRune(runeType);
            yield return new WaitForSeconds(delayBetweenRunes);
        }

        isCombatActive = false;
    }

    /// <summary>
    /// Procesa una sola runa a través de todos los enemigos
    /// </summary>
    private IEnumerator ProcessSingleRune(RuneType runeType)
    {
        Debug.Log($"Procesando runa: {runeType}");

        List<GameObject> enemiesObj = enemies.ToList<GameObject>();

        // Recorrer cada enemigo y comprobar si la runa les afecta
        foreach (GameObject enemy in enemiesObj)
        {
            if (enemy == null) continue;

            
            if (enemy.TryGetComponent<IncomeRuneChecker>(out var runeChecker))
            {
                // Procesar la runa y comprobar si debe continuar al siguiente enemigo
                bool continueToNextEnemy = runeChecker.ProcessRune(runeType);

                if (!continueToNextEnemy)
                {
                    // La runa fue absorbida por un escudo, detener el procesamiento
                    Debug.Log($"La runa {runeType} fue absorbida y no continuará a los siguientes enemigos");
                    break;
                }
            }

            // Pequeño retraso para visualizar la runa moviéndose de un enemigo a otro
            yield return new WaitForSeconds(0.2f);
        }
    }

    /// <summary>
    /// Añadir un enemigo al gestor de combate
    /// </summary>
    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy) && enemy.GetComponent<IncomeRuneChecker>() != null)
        {
            enemies.Add(enemy);
        }
    }

    /// <summary>
    /// Eliminar un enemigo del gestor de combate
    /// </summary>
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public void ClearEnemies()
    {
        if(enemies!=null)enemies.Clear();
    }

    /// <summary>
    /// Establecer el orden de los enemigos (para el recorrido de runas)
    /// </summary>
    public void SetEnemiesOrder(List<GameObject> newOrder)
    {
        // Asegurar que todos los enemigos en el nuevo orden existen en nuestra lista actual
        if (newOrder.All(e => enemies.Contains(e)))
        {
            enemies = newOrder;
        }
        else
        {
            Debug.LogWarning("No se puede establecer el orden de enemigos: el nuevo orden contiene enemigos que no están en la lista actual");
        }
    }
}