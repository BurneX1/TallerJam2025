using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputs;

public class RuneKeyEvoker : MonoBehaviour, IRunesActions
{
    [Header("Configuración de Runas")]
    [SerializeField] private int minRuneCount = 2;
    [SerializeField] private int maxRuneCount = 6;
    [SerializeField] float cooldownPerRune = 0.5f;

    [Header("Información de Enfriamiento")]
    [SerializeField] private bool showCooldownDebug = true;

    private PlayerInputs playerInputs;
    private readonly List<RuneType> currentRuneSequence = new();

    // Variables del sistema de enfriamiento
    private bool isOnCooldown = false;
    private float cooldownTimeRemaining = 0f;

    // Método público para obtener el estado actual del cooldown
    public bool IsOnCooldown => isOnCooldown;

    // Método público para obtener el tiempo restante de cooldown
    public float CooldownTimeRemaining => cooldownTimeRemaining;

    // Método público para obtener el tiempo total de cooldown
    public float CooldownTotalTime { get; private set; }

    // Evento que se dispara cuando se envía una secuencia de runas
    public delegate void RuneSequenceSubmitted(List<RuneType> runeSequence);
    public event RuneSequenceSubmitted OnRuneSequenceSubmitted;

    // Evento para cuando el enfriamiento comienza/termina
    public delegate void RuneCooldownChanged(bool isActive, float duration);
    public event RuneCooldownChanged OnRuneCooldownChanged;

    // Evento para cuando se añade una runa a la secuencia
    public delegate void RuneAdded(RuneType runeType);
    public event RuneAdded OnRuneAdded;

    // Evento para cuando se limpia la secuencia de runas
    public delegate void RunesCleared();
    public event RunesCleared OnRunesCleared;

    void Awake()
    {
        playerInputs = new PlayerInputs();
        // Registrar los callbacks de las acciones de runas
        playerInputs.Runes.SetCallbacks(this);
    }

    private void OnEnable()
    {
        playerInputs.Runes.Enable();
        playerInputs.GameInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Runes.Disable();
        playerInputs.GameInputs.Disable();
    }

    private void Update()
    {
        // Actualizar el temporizador de enfriamiento si está activo
        if (isOnCooldown)
        {
            cooldownTimeRemaining -= Time.deltaTime;

            if (cooldownTimeRemaining <= 0)
            {
                EndCooldown();
            }
            else if (showCooldownDebug && Mathf.FloorToInt(cooldownTimeRemaining * 10) % 5 == 0)
            {
                // Solo registrar cada 0.5 segundos para evitar spam en la consola
                Debug.Log($"Tiempo de enfriamiento restante: {cooldownTimeRemaining:F1} segundos");
            }
        }
    }

    // Implementando los callbacks de las acciones de runas
    #region IRunesActions implementation
    public void OnRune1(InputAction.CallbackContext context)
    {
        if (context.performed && !isOnCooldown && currentRuneSequence.Count < maxRuneCount)
        {
            AddRuneToSequence(RuneType.Rune1);
        }
    }

    public void OnRune2(InputAction.CallbackContext context)
    {
        if (context.performed && !isOnCooldown && currentRuneSequence.Count < maxRuneCount)
        {
            AddRuneToSequence(RuneType.Rune2);
        }
    }

    public void OnRune3(InputAction.CallbackContext context)
    {
        if (context.performed && !isOnCooldown && currentRuneSequence.Count < maxRuneCount)
        {
            AddRuneToSequence(RuneType.Rune3);
        }
    }

    public void OnLaunchRunes(InputAction.CallbackContext context)
    {
        if (context.performed && !isOnCooldown)
        {
            SubmitRuneSequence();
        }
        else if (context.performed && isOnCooldown)
        {
            Debug.LogWarning($"No se pueden enviar runas - en enfriamiento por {cooldownTimeRemaining:F1} segundos más");
        }
    }
    #endregion

    private void AddRuneToSequence(RuneType runeType)
    {
        // Añade la runa a la secuencia actual
        currentRuneSequence.Add(runeType);
        // Log de la runa añadida y la cantidad de runas en la secuencia actual
        Debug.Log($"Runa añadida: {runeType}. Cantidad en secuencia actual: {currentRuneSequence.Count}");

        // Disparar el evento OnRuneAdded
        OnRuneAdded?.Invoke(runeType);
    }

    private void SubmitRuneSequence()
    {
        if (currentRuneSequence.Count >= minRuneCount)
        {
            // Log de la cantidad de runas en la secuencia actual
            Debug.Log($"Enviando secuencia de runas con {currentRuneSequence.Count} runas");

            List<RuneType> submittedSequence = new(currentRuneSequence);
            OnRuneSequenceSubmitted?.Invoke(submittedSequence);

            // Calcula el tiempo de enfriamiento basado en la cantidad de runas en la secuencia actual
            float cooldownTime = CalculateCooldownTime(currentRuneSequence.Count);
            StartCooldown(cooldownTime);

            ClearRuneSequence();
        }
        else
        {
            Debug.LogWarning($"No hay suficientes runas para enviar. Se necesitan al menos {minRuneCount}, se tienen {currentRuneSequence.Count}");
        }
    }

    /// <summary>
    /// Calcula el tiempo de enfriamiento basado en la cantidad de runas en la secuencia actual
    /// </summary>
    /// <param name="runeCount">La cantidad de runas en la secuencia actual</param>
    /// <returns>El tiempo de enfriamiento en segundos</returns>
    float CalculateCooldownTime(int runeCount)
    {
        return runeCount * cooldownPerRune;
    }

    /// <summary>
    /// Inicia el periodo de enfriamiento
    /// </summary>
    /// <param name="duration">Duración del enfriamiento en segundos</param>
    private void StartCooldown(float duration)
    {
        isOnCooldown = true;
        cooldownTimeRemaining = duration;

        Debug.Log($"Iniciando enfriamiento por {duration:F1} segundos");

        // Notifica el inicio del enfriamiento
        OnRuneCooldownChanged?.Invoke(true, duration);
    }

    /// <summary>
    /// Finaliza el periodo de enfriamiento
    /// </summary>
    private void EndCooldown()
    {
        isOnCooldown = false;
        cooldownTimeRemaining = 0f;

        Debug.Log("Enfriamiento terminado - listo para nueva secuencia de runas");

        // Notifica el fin del enfriamiento
        OnRuneCooldownChanged?.Invoke(false, 0f);
    }

    private void ClearRuneSequence()
    {
        currentRuneSequence.Clear();
        Debug.Log("Secuencia de runas limpiada");

        // Disparar el evento OnRunesCleared
        OnRunesCleared?.Invoke();
    }

    // Método público para obtener la secuencia de runas actual en caso sea necesario
    public IReadOnlyList<RuneType> GetCurrentRuneSequence()
    {
        return currentRuneSequence.AsReadOnly();
    }
}