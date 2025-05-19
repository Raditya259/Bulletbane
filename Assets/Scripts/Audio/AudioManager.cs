using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] [Range(0f, 1f)] private float musicVolume = 0.5f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private AudioClip playerShootSound;
    [SerializeField] private AudioClip enemyShootSound;

    [Header("Volume Settings")]
    [SerializeField] [Range(0f, 1f)] private float sfxVolume = 0.8f;
    [SerializeField] [Range(0f, 1f)] private float playerShootVolume = 0.7f;
    [SerializeField] [Range(0f, 1f)] private float enemyShootVolume = 0.6f;
    [SerializeField] [Range(-0.3f, 0.3f)] private float pitchVariation = 0.1f;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource shootSource; // Dedicated to shooting sounds

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

        // Create audio sources
        musicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        shootSource = gameObject.AddComponent<AudioSource>();

        // Configure music source
        musicSource.clip = backgroundMusic;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        // Configure shooting source
        shootSource.volume = sfxVolume;
        shootSource.playOnAwake = false;
        shootSource.loop = false;
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic == null)
        {
            // Removed Debug.LogWarning
            return;
        }

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }

    public void PauseBackgroundMusic()
    {
        musicSource.Pause();
    }

    public void UnpauseBackgroundMusic()
    {
        musicSource.UnPause();
    }

    public void PlayVictorySound()
    {
        if (victorySound != null)
        {
            sfxSource.PlayOneShot(victorySound, sfxVolume);
        }
    }

    public void PlayDefeatSound()
    {
        if (defeatSound != null)
        {
            sfxSource.PlayOneShot(defeatSound, sfxVolume);
        }
    }
    
    public void PlayPlayerShootSound(Vector3 position)
    {
        if (playerShootSound != null)
        {
            shootSource.pitch = 1.0f; // Reset pitch
            shootSource.PlayOneShot(playerShootSound, playerShootVolume * sfxVolume);
        }
        else
        {
            // Removed Debug.LogWarning
        }
    }
    
    public void PlayEnemyShootSound(Vector3 position)
    {
        if (enemyShootSound != null)
        {
            // Apply random pitch variation to enemy shots for variety
            shootSource.pitch = 1.0f + Random.Range(-pitchVariation, pitchVariation);
            shootSource.PlayOneShot(enemyShootSound, enemyShootVolume * sfxVolume);
        }
        else
        {
            // Removed Debug.LogWarning
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
        // Shooting volume will scale with master SFX volume
    }
    
    public void SetPlayerShootVolume(float volume)
    {
        playerShootVolume = Mathf.Clamp01(volume);
    }
    
    public void SetEnemyShootVolume(float volume)
    {
        enemyShootVolume = Mathf.Clamp01(volume);
    }
}
