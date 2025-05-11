using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthChanged?.Invoke(currentHealth);

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
    }

    private void Die()
    {
        Debug.Log("Player died.");
        onPlayerDeath?.Invoke();
        gameObject.SetActive(false); // Atau trigger animasi, scene game over, dll.
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
