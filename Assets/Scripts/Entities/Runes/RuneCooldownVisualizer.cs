using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneCooldownVisualizer : MonoBehaviour
{
    [SerializeField] private Image cooldownBarImage;
    [SerializeField] private float visualRefreshSpeed = 5f;
    [SerializeField] private bool reverseFill = true;

    private RuneKeyEvoker runeKeyEvoker;
    private bool isOnCooldown = false;
    private float cooldownDuration = 0f;
    private float currentCooldown = 0f;

    private void Awake()
    {
        runeKeyEvoker = FindObjectOfType<RuneKeyEvoker>();

        // Inicializar elementos de UI
        if (cooldownBarImage != null)
        {
            cooldownBarImage.fillAmount = reverseFill ? 1f : 0f;
        }
    }

    private void OnEnable()
    {
        if (runeKeyEvoker != null)
        {
            runeKeyEvoker.OnRuneCooldownChanged += HandleCooldownChanged;
        }
    }

    private void OnDisable()
    {
        if (runeKeyEvoker != null)
        {
            runeKeyEvoker.OnRuneCooldownChanged -= HandleCooldownChanged;
        }
    }

    private void Update()
    {
        if (isOnCooldown && cooldownBarImage != null)
        {
            // Obtener el cooldown actual directamente del RuneKeyEvoker
            if (runeKeyEvoker != null)
            {
                currentCooldown = runeKeyEvoker.CooldownTimeRemaining;
            }
            else
            {
                // Revertir a contar localmente si no se puede acceder al RuneKeyEvoker
                currentCooldown -= Time.deltaTime;
            }

            // Asegurar que no sea negativo
            currentCooldown = Mathf.Max(0f, currentCooldown);

            // Actualizar UI
            UpdateCooldownBar();

            // Verificar si el enfriamiento terminó
            if (currentCooldown <= 0f)
            {
                isOnCooldown = false;

                // Resetear UI
                cooldownBarImage.fillAmount = reverseFill ? 1f : 0f;
            }
        }
    }

    private void HandleCooldownChanged(bool active, float duration)
    {
        isOnCooldown = active;
        cooldownDuration = duration;
        currentCooldown = duration;

        // Inicializar UI basado en estado de enfriamiento
        if (!active && cooldownBarImage != null)
        {
            cooldownBarImage.fillAmount = reverseFill ? 1f : 0f;
        }
    }

    private void UpdateCooldownBar()
    {
        if (cooldownBarImage != null && cooldownDuration > 0)
        {
            // Calcular el valor de relleno basado en el tiempo restante
            float fillRatio = currentCooldown / cooldownDuration;

            // Invertir si es necesario
            if (reverseFill)
            {
                fillRatio = 1f - fillRatio;
            }

            // Aplicar directamente sin Lerp
            cooldownBarImage.fillAmount = fillRatio;

            // Debug si es necesario
            //Debug.Log($"Cooldown: {currentCooldown:F1}/{cooldownDuration:F1}, Fill: {cooldownBarImage.fillAmount:F2}");
        }
    }
}