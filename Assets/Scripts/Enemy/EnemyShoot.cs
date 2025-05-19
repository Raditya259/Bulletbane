//EnemyShoot.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float timeBetweenShots = 1f;
    [SerializeField] private Animator muzzleFlashAnimator;

    // Keep this for backward compatibility and editor assignment
    [SerializeField] private AudioClip shootSFX;

    private float lastShotTime;

    private void Start()
    {
        // Check if AudioManager exists and there's no sound assigned there
        if (AudioManager.Instance != null && shootSFX != null)
        {
            Debug.Log("Consider assigning enemy shoot sound in AudioManager instead of EnemyShoot component");
        }
    }

    private void Update()
    {
        if (Time.time - lastShotTime >= timeBetweenShots)
        {
            Fire();
            lastShotTime = Time.time;
        }
    }

    private void Fire()
    {
        Vector3 shootDirection = gunTransform.right;
        shootDirection.z = 0;
        shootDirection = shootDirection.normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDirection * bulletSpeed;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.owner = BulletOwner.Enemy;
        }

        if (muzzleFlashAnimator != null)
        {
            muzzleFlashAnimator.SetTrigger("shoot");
        }

        // Play shooting sound through AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyShootSound(transform.position);
        }
    }
}