using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f; // Time in seconds between each shot
    public float detectionRadius = 10f;

    private float nextFireTime = 0f;
    private Transform target;

    void Update()
    {
        FindTarget();
        if (target != null && Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = detectionRadius;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy;
    }

    void Shoot()
    {
        // Instantiate the bullet at the firePoint position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Set the bullet target
        HomingBullet homingBullet = bullet.GetComponent<HomingBullet>();
        if (homingBullet != null && target != null)
        {
            homingBullet.SetTarget(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to represent the detection radius in the scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
