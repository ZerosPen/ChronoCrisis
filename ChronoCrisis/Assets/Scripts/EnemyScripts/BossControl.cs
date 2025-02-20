using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform player;
    public Collider2D bossCollider;
    public GameObject spearPrefab, aoePrefab, bulletPrefab, meleePrefab, darkCageBulletPrefab, missilePrefab, monsterPrefab;
    public Transform[] aoeSpawnPoints, bulletSpawnPoints;

    public float spearSpeedSlow = 5f, spearSpeedFast = 10f, bulletSpeed = 6f, meleeRange = 1f, teleportRange = 5f, phaseTwoMoveSpeed = 4f;
    public float phaseOneDuration = 300f;

    private bool isPhaseTwo = false;
    private bool attackRunning = false;
    private float phaseOneTimer;
    private List<Vector3> recentAttackPositions = new List<Vector3>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        phaseOneTimer = phaseOneDuration;
        bossCollider.enabled = false;
        StartCoroutine(PhaseOneAttackPattern());
    }

    void Update()
    {
        if (!isPhaseTwo)
        {
            phaseOneTimer -= Time.deltaTime;
            if (phaseOneTimer <= 0)
            {
                EnterPhaseTwo();
            }
        }
        else
        {
            MoveBoss();
        }
    }

    IEnumerator PhaseOneAttackPattern()
    {
        while (!isPhaseTwo)
        {
            attackRunning = true;
            int attack = Random.Range(0, 4);
            float interval = Random.Range(0.5f, 1f);

            switch (attack)
            {
                case 0: yield return SpearShoot(); break;
                case 1: yield return MeteorFall(); break;
                case 2: yield return DarkCage(); break;
                case 3: yield return MissileLaunch(); break;
            }
            
            if (Random.value < 0.25f) yield return TeleportPlayerNearAttack();
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator PhaseTwoAttackPattern()
    {
        while (isPhaseTwo)
        {
            attackRunning = true;
            if (Vector2.Distance(transform.position, player.position) > teleportRange)
            {
                SelfTeleportNearPlayer();
                yield return MeleeAttack();
            }
            else
            {
                SelfTeleportAway();
                int attack = Random.Range(0, 4);
                switch (attack)
                {
                    case 0: yield return SpearShoot(); break;
                    case 1: yield return MeteorFall(); break;
                    case 2: yield return DarkCage(); break;
                    case 3: yield return MissileLaunch(); break;
                }
            }
            
            if (Random.value < 0.25f) yield return TeleportPlayerNearAttack();
            yield return new WaitForSeconds(1.5f);
            if (Random.value < 0.3f) yield return SummonMonsters();
        }
    }

    IEnumerator SpearShoot()
    {
        GameObject spear = Instantiate(spearPrefab, transform.position, Quaternion.identity);
        float speed = Random.value > 0.5f ? spearSpeedSlow : spearSpeedFast;
        spear.GetComponent<Rigidbody2D>().velocity = (player.position - transform.position).normalized * speed;
        recentAttackPositions.Add(spear.transform.position);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator MeteorFall()
    {
        foreach (var point in aoeSpawnPoints)
        {
            GameObject aoe = Instantiate(aoePrefab, point.position, Quaternion.identity);
            recentAttackPositions.Add(point.position);
            Destroy(aoe, 1.5f);
        }
        yield return new WaitForSeconds(1f);
    }

    IEnumerator DarkCage()
    {
        Vector3 playerPosition = player.position;
        for (int i = 0; i < 8; i++)
        {
            float angle = i * 45f;
            Vector3 spawnPosition = playerPosition + Quaternion.Euler(0, 0, angle) * Vector3.right * 2f;
            GameObject bullet = Instantiate(darkCageBulletPrefab, spawnPosition, Quaternion.identity);
            recentAttackPositions.Add(spawnPosition);
            StartCoroutine(DelayedBulletMove(bullet, playerPosition));
        }
        yield return new WaitForSeconds(1f);
    }

    IEnumerator MissileLaunch()
    {
        int waves = Random.Range(50,101);
        float angleOffset = 0;
        for (int w = 0; w < waves; w++)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f + angleOffset;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;
                GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
                missile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                recentAttackPositions.Add(missile.transform.position);
                angleOffset += Random.Range(10f,15f);
            }
            
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator TeleportPlayerNearAttack()
    {
        if (recentAttackPositions.Count > 0)
        {
            Vector3 attackPos = recentAttackPositions[Random.Range(0, recentAttackPositions.Count)];
            Vector3 newPosition = attackPos + (Vector3)(Random.insideUnitCircle.normalized * 1.5f);
            player.position = newPosition;
            Debug.Log("Player teleported near attack!");
        }
        yield return null;
    }

    IEnumerator DelayedBulletMove(GameObject bullet, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(1f);
        if (bullet != null)
        {
            Vector2 direction = (targetPosition - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }
    }

    IEnumerator SummonMonsters()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
    }

    IEnumerator MeleeAttack()
    {
        if (Vector2.Distance(transform.position, player.position) <= meleeRange)
        {
            Instantiate(meleePrefab, transform.position, Quaternion.identity);
            Debug.Log("Boss uses melee attack!");
        }
        yield return new WaitForSeconds(0.5f);
    }

    void MoveBoss()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * phaseTwoMoveSpeed;
    }

    void SelfTeleportNearPlayer()
    {
        transform.position = player.position + (Vector3)(Random.insideUnitCircle.normalized * 1.5f);
        Debug.Log("Boss teleported near player!");
    }

    void SelfTeleportAway()
    {
        Vector3 awayDirection = (transform.position - player.position).normalized;
        transform.position += awayDirection * teleportRange;
        Debug.Log("Boss teleported away!");
    }

    public void EnterPhaseTwo()
    {
        isPhaseTwo = true;
        bossCollider.enabled = true;
        attackRunning = false;
        StopAllCoroutines();
        StartCoroutine(PhaseTwoAttackPattern());
    }
}