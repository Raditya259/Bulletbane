using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int maxHealth = 2000;
    [SerializeField] private int currentHealth;

    [Header("Events")]
    public UnityEvent onPlayerDeath;
    public UnityEvent<int> onHealthChanged;

    private UIManager uiManager;

    private void Awake()
    {
        currentHealth = maxHealth;
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        // Setup UI pertama kali
        if (uiManager != null)
        {
            uiManager.UpdateMaxHealth(currentHealth, maxHealth);
        }

        // Panggil event
        onHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth);
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        onHealthChanged?.Invoke(currentHealth);
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth);
        }
    }

    private void Die()
    {
        onPlayerDeath?.Invoke();

        // Notify GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDied();
        }

        gameObject.SetActive(false);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
