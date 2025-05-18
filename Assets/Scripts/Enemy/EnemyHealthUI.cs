using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private Gradient colorGradient;

    private Transform targetEnemy;
    private int maxHealth;
    private Vector3 offset;
    private RectTransform rectTransform;

    private void Awake()
    {
        if (healthSlider == null)
            healthSlider = GetComponentInChildren<Slider>();

        if (fillImage == null && healthSlider != null)
            fillImage = healthSlider.fillRect.GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(Transform enemy, int maxHp, Vector3 positionOffset)
    {
        targetEnemy = enemy;
        maxHealth = maxHp;
        offset = positionOffset;

        // Debug log to verify initialization
        Debug.Log($"EnemyHealthUI initialized for {enemy.name} with max health: {maxHp}");

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        if (fillImage != null && colorGradient != null)
        {
            fillImage.color = colorGradient.Evaluate(1f);
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;

            if (fillImage != null && colorGradient != null)
            {
                float healthPercentage = (float)currentHealth / maxHealth;
                fillImage.color = colorGradient.Evaluate(healthPercentage);
            }
        }
    }

    private void LateUpdate()
    {
        if (targetEnemy != null)
        {
            // Position the UI above the enemy
            transform.position = targetEnemy.position + offset;
        }
    }
}
