using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer runeRenderer;

    [Header("Configuración Visual")]
    [SerializeField] private AnimationCurve pulseScale = AnimationCurve.EaseInOut(0f, 1f, 1f, 1.2f);
    [SerializeField] private float pulseDuration = 1f;

    private float pulseTimer = 0f;
    private bool isPulsing = false;
    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnEnable()
    {        
        StartPulse();
    }

    private void OnDisable()
    {
        StopPulse();
    }

    private void Update()
    {
        if (isPulsing)
        {
            pulseTimer += Time.deltaTime;

            // Obtiene la escala desde la curva de animación
            float scale = pulseScale.Evaluate(pulseTimer / pulseDuration);
            transform.localScale = new Vector3(scale, scale, scale);

            // Reiniciar el temporizador cuando se alcanza la duración
            if (pulseTimer >= pulseDuration)
            {
                pulseTimer = 0f;
            }
        }
    }

    public void StartPulse()
    {
        isPulsing = true;
        pulseTimer = 0f;
    }

    public void StopPulse()
    {
        isPulsing = false;
        transform.localScale = originalScale;
    }

    
}