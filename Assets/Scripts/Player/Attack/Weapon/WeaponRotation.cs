using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    public float detectionRadius = 10f;
    private Transform target;

    void Update()
    {
        FindTarget();
        if (target != null)
        {
            RotateTowardsTarget();
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

    void RotateTowardsTarget()
    {
        Vector2 origin = transform.position;
        Vector2 targetPosition = target.position;
        
        // Calculate the direction from the origin to the target
        Vector2 direction = (targetPosition - origin).normalized;
        
        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rotate the weapon to face the target
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
