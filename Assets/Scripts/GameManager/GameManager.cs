using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    private bool isGameOver = false;
    private int totalEnemies = 0;
    private int enemiesDefeated = 0;

    [Header("UI References")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TMPro.TextMeshProUGUI enemyCountText; // Optional, untuk menampilkan sisa enemy

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1f;

        if (winScreen) winScreen.SetActive(false);
        if (loseScreen) loseScreen.SetActive(false);

        // Count all enemies that are already in the scene at start
        StartCoroutine(CountInitialEnemiesInScene());
    }

    private IEnumerator CountInitialEnemiesInScene()
    {
        // Wait a frame to ensure all enemies are properly initialized
        yield return null;
        
        // Find all enemies in scene
        EnemyStats[] allEnemies = FindObjectsOfType<EnemyStats>();
        totalEnemies = allEnemies.Length;
        enemiesDefeated = 0;
        
        Debug.Log($"Found {totalEnemies} enemies in the scene");
        UpdateEnemyCountUI();
    }
    
    // Register a single enemy (used when enemies spawn one by one)
    public void RegisterEnemy()
    {
        totalEnemies++;
        UpdateEnemyCountUI();
    }

    public void OnEnemyKilled()
    {
        if (isGameOver) return;

        enemiesDefeated++;
        UpdateEnemyCountUI();

        Debug.Log($"Enemy killed: {enemiesDefeated}/{totalEnemies}");

        // Check if all enemies are defeated
        if (enemiesDefeated >= totalEnemies && totalEnemies > 0)
        {
            WinGame();
        }
    }

    private void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies: {totalEnemies - enemiesDefeated}/{totalEnemies}";
        }
    }

    public void OnPlayerDied()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;
        if (loseScreen) loseScreen.SetActive(true);

        Debug.Log("GAME OVER - PLAYER DIED");
    }

    private void WinGame()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        if (winScreen) winScreen.SetActive(true);

        Debug.Log("YOU WIN!");
    }

    // Button methods for UI
    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}
