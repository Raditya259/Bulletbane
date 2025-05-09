//EnemyGunController.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    [SerializeField] private Transform gun;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float orbitRadius = 2f;

    private void Update()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;

        Vector3 gunOffset = direction * orbitRadius;
        gun.position = transform.position + gunOffset;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 scale = gun.localScale;
        scale.y = direction.x < 0 ? -1 : 1;
        gun.localScale = scale;
    }
}