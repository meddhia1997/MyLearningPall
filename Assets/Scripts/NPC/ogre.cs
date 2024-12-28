using UnityEngine;

public class ogre : MonoBehaviour
{
    public float activationDistance = 1f; // Distance to activate attack animation
    public Animator animator;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Me").transform;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= activationDistance)
            {
                // Activate attack animation trigger
                animator.SetTrigger("Attack");
            }
        }
    }
}
