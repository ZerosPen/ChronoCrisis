using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float HitPoint = 100;
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float MovementChase = 2.5f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float stopDuration = 3f;

    [SerializeField] private Transform player;
    private Rigidbody2D rb;
    private bool isChasing = false;
    private bool isStopped = false;

    [SerializeField] private Transform waypointsA;
    [SerializeField] private Transform waypointsB;
    private Vector2 targetWayPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
        SetNewTargetPos();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && !isStopped)
        {
            if (!isChasing)
            {
                isChasing = true;
                Debug.Log("Enemy is now chasing the player!");
            }
            ChaseTarget();
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                Debug.Log("Enemy stopped chasing.");
            }

            if (!isStopped)
            {
                MoveTowardTarget();
            }
        }
}

    void SetNewTargetPos()
    {
        if (waypointsA == null || waypointsB == null)
        {
            Debug.LogError("Waypoints are not assigned!");
            return;
        }

        float randomX = Random.Range(waypointsA.position.x, waypointsB.position.x);
        float randomY = Random.Range(waypointsA.position.y, waypointsB.position.y);

        targetWayPoint = new Vector2(randomX, randomY);
    }

    private void MoveTowardTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetWayPoint, MovementSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetWayPoint) < 0.1f)
        {
            isStopped = true;
            Invoke("ResumePatrol", stopDuration);
        }
    }

    private void ChaseTarget()
    {
       if(isChasing == true)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, MovementSpeed * MovementChase * Time.deltaTime);
        }
    }

    private void ResumePatrol()
    {
        SetNewTargetPos();
        isStopped = false;
    }

    public void EnemyTakeDamage(float damage)
    {
        HitPoint -= damage;
        Debug.Log($"HP enemy {HitPoint}");

        if (HitPoint <= 0)
        {
            EnemyDead();
        }
    }

    private void EnemyDead()
    {
        isStopped = true;

        // Ensure Rigidbody stops moving before disabling the object
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        Debug.Log("Enemy Dead");

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
