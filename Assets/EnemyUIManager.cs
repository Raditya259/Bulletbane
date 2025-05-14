using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIManager : MonoBehaviour
{
    [Header("Enemy Health UI")]
    [SerializeField] private Slider healthSlider;

    // Dipanggil saat enemy spawn atau diinisialisasi
    public void SetupHealth(int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }
    }

    // Dipanggil setiap enemy menerima damage
    public void UpdateHealth(int currentHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    // Opsional: Untuk menyembunyikan UI saat mati
    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }
}
