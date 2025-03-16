using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputs;

public class RuneKeyEvoker : MonoBehaviour, IRunesActions
{
    [Header("Rune Configuration")]
    [SerializeField] private int minRuneCount = 2;
    [SerializeField] private int maxRuneCount = 6;
    [SerializeField] float cooldownPerRune = 0.5f;

    [Header("Cooldown Feedback")]
    [SerializeField] private bool showCooldownDebug = true;

    private PlayerInputs playerInputs;
    private readonly List<RuneType> currentRuneSequence = new();

    // Cooldown system variables
    private bool isOnCooldown = false;
    private float cooldownTimeRemaining = 0f;

    // Evento que se dispara cuando se envía una secuencia de runas
    public delegate void RuneSequenceSubmitted(List<RuneType> runeSequence);
    public event RuneSequenceSubmitted OnRuneSequenceSubmitted;

    // Event for cooldown started/ended
    public delegate void RuneCooldownChanged(bool isActive, float duration);
    public event RuneCooldownChanged OnRuneCooldownChanged;

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
        // Update cooldown timer if active
        if (isOnCooldown)
        {
            cooldownTimeRemaining -= Time.deltaTime;

            if (cooldownTimeRemaining <= 0)
            {
                EndCooldown();
            }
            else if (showCooldownDebug && Mathf.FloorToInt(cooldownTimeRemaining * 10) % 5 == 0)
            {
                // Only log every 0.5 seconds to avoid console spam
                Debug.Log($"Cooldown remaining: {cooldownTimeRemaining:F1} seconds");
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
            Debug.LogWarning($"Cannot submit runes - on cooldown for {cooldownTimeRemaining:F1} more seconds");
        }
    }
    #endregion

    private void AddRuneToSequence(RuneType runeType)
    {
        // Añade la runa a la secuencia actual
        currentRuneSequence.Add(runeType);
        // Log de la runa añadida y la cantidad de runas en la secuencia actual
        Debug.Log($"Rune added: {runeType}. Current sequence count: {currentRuneSequence.Count}");
    }

    private void SubmitRuneSequence()
    {
        if (currentRuneSequence.Count >= minRuneCount)
        {
            // Log de la cantidad de runas en la secuencia actual
            Debug.Log($"Submitting rune sequence with {currentRuneSequence.Count} runes");

            List<RuneType> submittedSequence = new(currentRuneSequence);
            OnRuneSequenceSubmitted?.Invoke(submittedSequence);

            // Calcula el tiempo de enfriamiento basado en la cantidad de runas en la secuencia actual
            float cooldownTime = CalculateCooldownTime(currentRuneSequence.Count);
            StartCooldown(cooldownTime);

            ClearRuneSequence();
        }
        else
        {
            Debug.LogWarning($"Not enough runes to submit. Need at least {minRuneCount}, have {currentRuneSequence.Count}");
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

        Debug.Log($"Starting cooldown for {duration:F1} seconds");

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

        Debug.Log("Cooldown ended - ready for new rune sequence");

        // Notifica el fin del enfriamiento
        OnRuneCooldownChanged?.Invoke(false, 0f);
    }

    private void ClearRuneSequence()
    {
        currentRuneSequence.Clear();
        Debug.Log("Rune sequence cleared");
    }

    // Metodo publico para obtener la secuencia de runas actual en caso sea necesario
    public IReadOnlyList<RuneType> GetCurrentRuneSequence()
    {
        return currentRuneSequence.AsReadOnly();
    }
}