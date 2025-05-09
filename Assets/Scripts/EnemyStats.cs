using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private int maxHealth = 500;
    [SerializeField] private int currentHealth;

    [Header("Events")]
    public UnityEvent onEnemyDeath;
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

    private void Die()
    {
        // Tambahkan efek screen shake atau animasi di sini
        Debug.Log($"{gameObject.name} died.");
        onEnemyDeath?.Invoke();
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
    }
}