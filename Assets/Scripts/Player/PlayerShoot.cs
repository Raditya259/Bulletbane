using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float timeBetweenShots = 0.1f;
    [SerializeField] private Animator muzzleFlashAnimator;
    
    // Keep this for backward compatibility and editor assignment
    [SerializeField] private AudioClip shootSFX;

    private float lastShotTime = 0f;
    private bool isFiring = false;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        
        // Check if AudioManager exists and there's no sound assigned there
        if (AudioManager.Instance != null && shootSFX != null)
        {
            Debug.Log("Consider assigning player shoot sound in AudioManager instead of PlayerShoot component");
        }
    }

    private void Update()
    {
        AimGun();

        if (isFiring && Time.time - lastShotTime >= timeBetweenShots)
        {
            Fire();
            lastShotTime = Time.time;
        }
    }

    private void AimGun()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseWorldPos.z = 0;

        Vector3 direction = (mouseWorldPos - transform.position).normalized;

        // Fixed distance from player to weapon position
        float radius = 1f; // Adjust orbit radius
        Vector3 gunOffset = direction * radius;

        // Place guns around the player
        gun.position = transform.position + gunOffset;

        // Rotate the gun to face the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0, 0, angle);

        // Flip gun if mouse direction is to the left (so the sprite doesn't reversed)
        Vector3 scale = gun.localScale;
        scale.y = direction.x < 0 ? -1 : 1;
        gun.localScale = scale;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isFiring = true;
        }
        else if (context.canceled)
        {
            isFiring = false;
        }
    }

    private void Fire()
    {
        Vector3 shootDir = gun.right;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = shootDir * bulletSpeed;

        // Muzzle flash
        if (muzzleFlashAnimator != null)
        {
            muzzleFlashAnimator.SetTrigger("shoot");
        }

        // Play shooting sound through AudioManager
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerShootSound(transform.position);
        }
        
        // No need to play the sound directly anymore
    }
}