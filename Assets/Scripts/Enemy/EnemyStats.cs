using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int currentHealth;

    [Header("UI")]
    [SerializeField] private GameObject healthUIPrefab;
    [SerializeField] private Vector3 healthUIOffset = new Vector3(0, 1.5f, 0);
    [SerializeField] private Canvas worldSpaceCanvas; // Reference to your Canvas2
    [SerializeField] private Vector3 healthUIScale = new Vector3(0.5f, 0.5f, 0.5f); // Control scale of health UI
    private EnemyHealthUI healthUI;
    private GameObject healthUIInstance;
    private bool healthUIVisible = false;

    [Header("Events")]
    public UnityEvent onEnemyDeath;
    public UnityEvent<int> onHealthChanged;

    private void Awake()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }

    private void Start()
    {
        // No need to register with GameManager manually anymore
        // GameManager will find all EnemyStats components in the scene

        // Create health UI for this enemy but keep it hidden initially
        if (healthUIPrefab != null)
        {
            // Find the world space canvas if not set
            if (worldSpaceCanvas == null)
            {
                // Try to find Canvas2 in the scene
                Canvas[] canvases = FindObjectsOfType<Canvas>();
                foreach (var canvas in canvases)
                {
                    if (canvas.renderMode == RenderMode.WorldSpace)
                    {
                        worldSpaceCanvas = canvas;
                        break;
                    }
                }

                if (worldSpaceCanvas == null)
                {
                    // Removed Debug.LogError and replaced with more silent handling
                    return;
                }
            }

            // Instantiate as a child of the world space canvas
            healthUIInstance = Instantiate(healthUIPrefab, transform.position + healthUIOffset, Quaternion.identity, worldSpaceCanvas.transform);

            // Set the scale to make the health UI smaller
            healthUIInstance.transform.localScale = healthUIScale;

            healthUI = healthUIInstance.GetComponent<EnemyHealthUI>();

            if (healthUI != null)
            {
                healthUI.Initialize(transform, maxHealth, healthUIOffset);
                // Make sure it's deactivated initially
                healthUIInstance.SetActive(false);
            }
            else
            {
                // Removed Debug.LogError
            }
        }
    }

    public void TakeDamage(int amount)
    {
        // Removed Debug.Log

        int previousHealth = currentHealth;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth);

        // Show and update health UI when damage is taken
        if (healthUI != null)
        {
            // Removed Debug.Log

            // If health UI is not visible yet and health decreased
            if (!healthUIVisible && currentHealth < previousHealth)
            {
                // Removed Debug.Log
                healthUI.gameObject.SetActive(true);
                healthUIVisible = true;
            }

            healthUI.UpdateHealth(currentHealth);
        }
        else
        {
            // Removed Debug.LogError
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Removed Debug.Log
        onEnemyDeath?.Invoke();

        // Notify the GameManager that an enemy has been killed
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnemyKilled();
        }

        // Destroy the health UI
        if (healthUI != null)
        {
            Destroy(healthUI.gameObject);
        }

        Destroy(gameObject);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    // Untuk membedakan Boss
    public void SetMaxHealth(int newMax)
    {
        maxHealth = newMax;
        currentHealth = newMax;
        onHealthChanged?.Invoke(currentHealth);

        // Update health UI if it exists
        if (healthUI != null)
        {
            healthUI.Initialize(transform, maxHealth, healthUIOffset);
            healthUI.gameObject.SetActive(false); // Keep hidden until damaged
        }
    }
}