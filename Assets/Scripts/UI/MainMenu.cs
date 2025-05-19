using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Optional Audio")]
    [SerializeField] private AudioClip buttonClickSound;

    private AudioSource audioSource;

    private void Start()
    {
        // Set up audio source if needed
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && buttonClickSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Make sure main menu panel is active at start
        ShowMainMenu();
    }

    public void PlayGame()
    {
        PlayButtonSound();
        SceneManager.LoadScene("SampleScene"); // Change to your game scene name
    }

    public void OpenSettings()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        PlayButtonSound();
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        PlayButtonSound();
    }

    public void QuitGame()
    {
        PlayButtonSound();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void PlayButtonSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
