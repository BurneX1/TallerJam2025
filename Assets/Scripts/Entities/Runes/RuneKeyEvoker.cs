using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerInputs;

public class RuneKeyEvoker : MonoBehaviour, IRunesActions
{
    [Header("Rune Configuration")]
    [SerializeField] private int minRuneCount = 2;
    [SerializeField] private int maxRuneCount = 6;

    private PlayerInputs playerInputs;
    private readonly List<RuneType> currentRuneSequence = new();

    // Evento que se dispara cuando se envía una secuencia de runas
    public delegate void RuneSequenceSubmitted(List<RuneType> runeSequence);
    public event RuneSequenceSubmitted OnRuneSequenceSubmitted;

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

    // Implementando los callbacks de las acciones de runas
    #region IRunesActions implementation
    public void OnRune1(InputAction.CallbackContext context)
    {
        if (context.performed && currentRuneSequence.Count < maxRuneCount)
        {
            AddRuneToSequence(RuneType.Rune1);
        }
    }

    public void OnRune2(InputAction.CallbackContext context)
    {
        if (context.performed && currentRuneSequence.Count < maxRuneCount)
        {
            AddRuneToSequence(RuneType.Rune2);
        }
    }

    public void OnRune3(InputAction.CallbackContext context)
    {
        if (context.performed && currentRuneSequence.Count < maxRuneCount)
        {
            AddRuneToSequence(RuneType.Rune3);
        }
    }
        
    public void OnLaunchRunes(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SubmitRuneSequence();
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
        // Chequea si la cantidad de runas en la secuencia actual es suficiente para enviar
        if (currentRuneSequence.Count >= minRuneCount)
        {
            // Log de la cantidad de runas en la secuencia actual
            Debug.Log($"Submitting rune sequence with {currentRuneSequence.Count} runes");

            List<RuneType> submittedSequence = new(currentRuneSequence);
            OnRuneSequenceSubmitted?.Invoke(submittedSequence);

            ClearRuneSequence();
        }
        else
        {
            Debug.LogWarning($"Not enough runes to submit. Need at least {minRuneCount}, have {currentRuneSequence.Count}");
        }
    }

    private void ClearRuneSequence()
    {
        currentRuneSequence.Clear();
        Debug.Log("Rune sequence cleared");
    }

    // Metodo publico para obtener la secuencia de runas actual
    public IReadOnlyList<RuneType> GetCurrentRuneSequence()
    {
        return currentRuneSequence.AsReadOnly();
    }
}