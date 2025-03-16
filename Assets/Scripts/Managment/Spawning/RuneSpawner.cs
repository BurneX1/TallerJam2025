using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneSpawner : MonoBehaviour
{
    [Header("Configuración del Spawner")]
    [SerializeField] private SpawnPoolManager poolManager;
    [SerializeField] private Transform[] runeSlots; // Array de posiciones donde se generarán las runas
    [SerializeField] private string[] runeObjectNames; // Los nombres de los objetos de runa en SpawnPoolManager

    [Header("Configuración Visual")]
    [SerializeField] private float runeSpawnAnimationDuration = 0.3f;
    [SerializeField] private float runeDespawnAnimationDuration = 0.2f;

    private RuneKeyEvoker runeKeyEvoker;
    private readonly List<GameObject> spawnedRunes = new();
    private readonly Dictionary<RuneType, string> runeTypeToObjectName = new();

    private void Awake()
    {
        // Obtener el componente RuneKeyEvoker
        runeKeyEvoker = FindObjectOfType<RuneKeyEvoker>();

        // Mapear los valores enum de RuneType a nombres de objetos en SpawnPoolManager
        // Asumiendo que el array runeObjectNames tiene nombres en el mismo orden que el enum RuneType
        for (int i = 0; i < runeObjectNames.Length && i < System.Enum.GetValues(typeof(RuneType)).Length; i++)
        {
            runeTypeToObjectName[(RuneType)i] = runeObjectNames[i];
        }
    }

    private void OnEnable()
    {
        if (runeKeyEvoker != null)
        {
            // Suscribirse a los eventos de RuneKeyEvoker
            runeKeyEvoker.OnRuneSequenceSubmitted += ClearSpawnedRunes;

            // Estos son eventos personalizados que necesitamos agregar a RuneKeyEvoker
            runeKeyEvoker.OnRuneAdded += SpawnRune;
        }
    }

    private void OnDisable()
    {
        if (runeKeyEvoker != null)
        {
            // Desuscribirse de los eventos de RuneKeyEvoker
            runeKeyEvoker.OnRuneSequenceSubmitted -= ClearSpawnedRunes;

            // Desuscribirse de nuestros eventos personalizados
            runeKeyEvoker.OnRuneAdded -= SpawnRune;
        }
    }

    /// <summary>
    /// Genera una representación visual de la runa en el siguiente slot disponible
    /// </summary>
    /// <param name="runeType">El tipo de runa a generar</param>
    private void SpawnRune(RuneType runeType)
    {
        // Verificar si tenemos slots disponibles
        if (spawnedRunes.Count >= runeSlots.Length)
        {
            Debug.LogWarning("No hay más slots de runas disponibles");
            return;
        }

        // Obtener el nombre del objeto para este tipo de runa
        if (!runeTypeToObjectName.TryGetValue(runeType, out string objectName))
        {
            Debug.LogError($"No hay mapeo de nombre de objeto para el RuneType: {runeType}");
            return;
        }

        // Encontrar el objeto en SpawnPoolManager
        int objectIndex = poolManager.GetObjectListValue(objectName);
        if (objectIndex < 0)
        {
            Debug.LogError($"Objeto '{objectName}' no encontrado en SpawnPoolManager");
            return;
        }

        // Obtener la posición del siguiente slot
        Transform slotTransform = runeSlots[spawnedRunes.Count];

        // Generar el objeto de runa en la posición del slot
        GameObject runeObject = poolManager.Generate(objectIndex, slotTransform.position, slotTransform);

        // Agregar a nuestra lista de runas generadas
        spawnedRunes.Add(runeObject);

        // Animar la aparición de la runa (opcional)
        StartCoroutine(AnimateRuneSpawn(runeObject));
    }

    /// <summary>
    /// Limpia todas las runas generadas
    /// </summary>
    /// <param name="runeSequence">Parámetro opcional para coincidir con la firma del evento</param>
    private void ClearSpawnedRunes(List<RuneType> runeSequence = null)
    {
        StartCoroutine(AnimateRuneClear());
    }

    /// <summary>
    /// Animación simple para escalar la runa al generarla
    /// </summary>
    private IEnumerator AnimateRuneSpawn(GameObject runeObject)
    {
        // Comenzar con escala 0
        runeObject.transform.localScale = Vector3.zero;

        float elapsedTime = 0f;
        while (elapsedTime < runeSpawnAnimationDuration)
        {
            // Escalar con el tiempo
            float scale = Mathf.Lerp(0, 1, elapsedTime / runeSpawnAnimationDuration);
            runeObject.transform.localScale = new Vector3(scale, scale, scale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurar que la escala final sea exactamente 1
        runeObject.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Animar la desaparición de todas las runas, luego desactivarlas
    /// </summary>
    private IEnumerator AnimateRuneClear()
    {
        float elapsedTime = 0f;
        List<GameObject> runesToClear = new(spawnedRunes);

        while (elapsedTime < runeDespawnAnimationDuration)
        {
            // Reducir escala con el tiempo
            float scale = Mathf.Lerp(1, 0, elapsedTime / runeDespawnAnimationDuration);

            foreach (GameObject rune in runesToClear)
            {
                if (rune != null)
                {
                    rune.transform.localScale = new Vector3(scale, scale, scale);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Desactivar todas las runas
        foreach (GameObject rune in runesToClear)
        {
            if (rune != null)
            {
                rune.SetActive(false);
            }
        }

        // Limpiar nuestra lista
        spawnedRunes.Clear();
    }
}