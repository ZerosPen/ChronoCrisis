using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header(("Status"))]
    public float HitPoint = 35;
    [SerializeField] private float MovementSpeed = 5f;
    private float MovementChase = 2.5f;
    private float chaseRange = 5f;
    private float attackRange = 5f;
    private float stopDuration = 3f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float AttackCoolDown = 5f;
    [SerializeField] private float physicalRes = 20;
    [SerializeField] private float magicRes = 20;
    public float currentHp;

    [Header("conditon & requimen")]
    [SerializeField] private Transform player;
    private Rigidbody2D rb;
    private bool isChasing = false;
    private bool isStopped = false;
    private bool isAttacking = false;
    [SerializeField] private Transform waypointsA;
    [SerializeField] private Transform waypointsB;
    private Vector2 targetWayPoint;
    private Skill skills;
    [SerializeField] private LayerMask playerLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
        currentHp = HitPoint;
        SetNewTargetPos();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            isChasing = false;  // Stop chasing when attacking
            isAttacking = true;
            AttackCoolDown -= Time.deltaTime;
            if (AttackCoolDown <= 0 && isAttacking)
            {
                AttackTarget();
            }
        }
        else if (distanceToPlayer <= chaseRange && !isAttacking && !isStopped)
        {
            isChasing = true;
            isAttacking = false;  // Ensure it is not attacking when chasing
            ChaseTarget();
        }

        else
        {
            isChasing = false;
            isAttacking = false;
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
        if (isChasing == true)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, MovementSpeed * MovementChase * Time.deltaTime);
        }
    }

    private void AttackTarget()
    {
        if (isAttacking == true)
        {
            Debug.Log("Enemy is attacking!");
            Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
            Debug.Log($"Player detected: {hitPlayer.Length}");

            foreach (Collider2D hitColliderPlayer in hitPlayer)
            {
                AttackCoolDown -= Time.deltaTime;
                if (AttackCoolDown <= 0 && isAttacking == true)
                {
                    if (hitColliderPlayer.gameObject.CompareTag("Player"))
                    {
                        PlayerController player = hitColliderPlayer.GetComponent<PlayerController>();
                        player.recievedDamage(attackDamage);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Enemy is not attacking!");
        }
    }

    private void ResumePatrol()
    {
        SetNewTargetPos();
        isStopped = false;
    }

    public void EnemyTakeDamageFormPlayer(float damage)
    {
        if (CompareTag("EnemyPhysical")) // Correct tag check
        {
            damage -= damage * (physicalRes / 100);
        }

        else if (CompareTag("EnemyMagic")) // Correct tag check
        {
            damage -= damage * (physicalRes / 100);
        }

        currentHp -= damage; // Subtract from current HP

        if (currentHp <= 0)
        {
            EnemyDead();
        }
    }

    public void EnemyTakeDamageFormSkill(float damage, string damageType)
    {
        if (damageType == "physical")
        {
            damage -= (damage * (physicalRes / 100));
            Debug.Log($"{damageType} {damage}");
        }
        else if (damageType == "magic")
        {
            damage -= (damage * (magicRes / 100));
            Debug.Log($"{damageType} {damage}");
        }

        currentHp -= damage;

        if (currentHp <= 0)
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
