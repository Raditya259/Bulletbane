using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    private bool isGameOver = false;

    [Header("UI References")]
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

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
    }

    public void OnEnemyKilled()
    {
        if (isGameOver) return;

        int enemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemiesLeft <= 1) // karena enemy ini belum di-destroy ketika dipanggil
        {
            WinGame();
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

    // Opsional: tombol UI
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
