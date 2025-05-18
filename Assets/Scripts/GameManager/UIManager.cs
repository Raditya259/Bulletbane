using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private Canvas screenSpaceCanvas; // Your regular Canvas
    [SerializeField] private Canvas worldSpaceCanvas; // Your Canvas2

    [Header("Player UI")]
    [Tooltip("The health slider")]
    [SerializeField]
    private Slider healthSlider;

    [Header("Enemy UI")]
    [SerializeField] private GameObject enemyHealthUIPrefab;

    private void Awake()
    {
        // If canvases are not assigned, try to find them in the scene
        if (screenSpaceCanvas == null || worldSpaceCanvas == null)
        {
            Canvas[] allCanvases = FindObjectsOfType<Canvas>();
            foreach (var canvas in allCanvases)
            {
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
                    canvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    screenSpaceCanvas = canvas;
                }
                else if (canvas.renderMode == RenderMode.WorldSpace)
                {
                    worldSpaceCanvas = canvas;
                }
            }
        }
    }

    public void UpdateMaxHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void UpdateHealth(int currentHealth)
    {
        healthSlider.value = currentHealth;
    }

    public GameObject CreateEnemyHealthUI()
    {
        if (enemyHealthUIPrefab != null && worldSpaceCanvas != null)
        {
            return Instantiate(enemyHealthUIPrefab, worldSpaceCanvas.transform);
        }
        return null;
    }

    public Canvas GetWorldSpaceCanvas()
    {
        return worldSpaceCanvas;
    }
}