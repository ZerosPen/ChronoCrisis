using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class BossControl : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform player;
    public Collider2D bossCollider;
    public GameObject spearPrefab, aoePrefab, bulletPrefab, meleePrefab, darkCageBulletPrefab, missilePrefab, monsterPrefab, aoeIndicatorPrefab;
    public GameObject teleportEffectPrefab;
    public GameObject staff;
    public Animator animator;

    private bool hasTriggeredPhaseTwoDialog = false;
    
    
    public Transform[] aoeSpawnPoints, bulletSpawnPoints;

    public float spearSpeedSlow = 5f, spearSpeedFast = 10f, bulletSpeed = 6f, meleeRange = 1f, teleportRange = 5f, phaseTwoMoveSpeed = 4f;
    public float phaseOneDuration = 300f;

    private bool isPhaseTwo = false;
    private bool isPhaseOne = true;
    private bool attackRunning = false;
    private float phaseOneTimer;
    private List<Vector3> recentAttackPositions = new List<Vector3>();
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueObject bossPhase2Dialogue;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        phaseOneTimer = phaseOneDuration;
        bossCollider.enabled = false;
        StartCoroutine(PhaseOneAttackPattern());
    }

    void Update()
    {
        if (player.position.x < transform.position.x){
            transform.localScale = new Vector3(1, 1, 1);
            staff.transform.position = new Vector3 (-2.5f, 100, -1);
            staff.transform.localScale = new Vector3 (1, 1, -1);
        }
        else{
            transform.localScale = new Vector3(-1, 1, 1);
            staff.transform.position = new Vector3 (2.5f, 100, -1);
            staff.transform.localScale = new Vector3 (-1, 1, -1);
        }

        if (!isPhaseTwo)
        {
            phaseOneTimer -= Time.deltaTime;
            if (phaseOneTimer <= 0)
            {
                isPhaseOne = false;
                StopAllCoroutines();
                if(!hasTriggeredPhaseTwoDialog){
                    TriggerPhaseTwoDialog();
                }
                
            }
        }
        else
        {
            MoveBoss();
        }
    }

    IEnumerator PhaseOneAttackPattern()
    {
        if(isPhaseOne == true){
            while (!isPhaseTwo)
            {
                attackRunning = true;
                animator.SetBool("Phase1Idle", false);

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

                animator.SetBool("Phase1Idle", true);
                yield return new WaitForSeconds(interval);
            }
        }
        
    }

void TriggerPhaseTwoDialog()
{
    if (hasTriggeredPhaseTwoDialog) return; // Prevent it from triggering again
    hasTriggeredPhaseTwoDialog = true;
    Debug.Log("Triggering Phase Two Dialogue");
    dialogueUI.ShowDialogue(bossPhase2Dialogue);
    StartCoroutine(WaitForDialogue());
}

    IEnumerator WaitForDialogue()
    {
        yield return new WaitUntil(() => !dialogueUI.IsOpen);
        Debug.Log("Dialogue closed, entering Phase Two");
        EnterPhaseTwo();
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
        Animator staffAnimator = staff.GetComponent<Animator>();
        if (staffAnimator != null) staffAnimator.SetTrigger("Casting");

        yield return new WaitForSeconds(0.5f);

        GameObject spear = Instantiate(spearPrefab, transform.position, Quaternion.identity);
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180;
        spear.transform.rotation = Quaternion.Euler(0, 0, angle);

        Animator spearAnimator = spear.GetComponent<Animator>();
        if (spearAnimator != null) spearAnimator.SetTrigger("Spear");

        float speed = Random.value > 0.5f ? spearSpeedSlow : spearSpeedFast;
        spear.GetComponent<Rigidbody2D>().velocity = direction * speed;

        recentAttackPositions.Add(spear.transform.position);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator MeteorFall()
    {
        Animator staffAnimator = staff.GetComponent<Animator>();
        if (staffAnimator != null) staffAnimator.SetTrigger("Casting");

        yield return new WaitForSeconds(0.5f);

        int meteorCount = 5;
        float spawnInterval = 0.5f;

        for (int i = 0; i < meteorCount; i++)
        {
            Animator bulletAnimator = aoePrefab.GetComponent<Animator>();
            if(bulletAnimator != null) bulletAnimator.SetTrigger("Bullet");
            Vector2 targetPosition = player.position;
            GameObject warning = Instantiate(aoeIndicatorPrefab, targetPosition, Quaternion.identity);
            yield return new WaitForSeconds(1f);

            GameObject meteor = Instantiate(aoePrefab, new Vector2(targetPosition.x, targetPosition.y + 100f), Quaternion.Euler(0, 0, 90));

            StartCoroutine(MoveMeteor(meteor, meteor.transform.position, targetPosition, 0.35f));

            Destroy(warning);
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveMeteor(GameObject meteor, Vector2 start, Vector2 target, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (meteor == null) yield break;

            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            meteor.transform.position = Vector2.Lerp(start, target, progress);

            yield return null;
        }

        if (meteor != null) 
        {
            meteor.transform.position = target;
        }
    }

    IEnumerator DarkCage()
    {
        Animator staffAnimator = staff.GetComponent<Animator>();
        if (staffAnimator != null) staffAnimator.SetTrigger("Casting");

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

    IEnumerator DelayedBulletMove(GameObject bullet, Vector3 targetPosition)
    {
        yield return new WaitForSeconds(0.5f);
        if (bullet != null)
        {
            Vector2 direction = (targetPosition - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        }
    }

    IEnumerator MissileLaunch()
    {
        Animator staffAnimator = staff.GetComponent<Animator>();
        if (staffAnimator != null) staffAnimator.SetTrigger("Casting");

        int waves = Random.Range(50, 101);
        float angleOffset = 0;
        for (int w = 0; w < waves; w++)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * 45f + angleOffset;
                Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

                // Instantiate missile
                GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);

                // Rotate missile to face movement direction
                float missileAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                missile.transform.rotation = Quaternion.Euler(0, 0, missileAngle+180);

                // Play animation on missile itself
                Animator missileAnimator = missile.GetComponent<Animator>();
                if (missileAnimator != null) missileAnimator.SetTrigger("Bullet");

                // Move missile in the correct direction
                missile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                recentAttackPositions.Add(missile.transform.position);
                angleOffset += Random.Range(10f, 15f);
            }

            yield return new WaitForSeconds(0.15f);
        }
    }


    IEnumerator TeleportPlayerNearAttack()
    {
        Animator staffAnimator = staff.GetComponent<Animator>();
        if (staffAnimator != null) staffAnimator.SetTrigger("Casting");
        
        if (recentAttackPositions.Count > 0)
        {
            animator.SetTrigger("Casting"); // Boss plays casting animation

            Vector3 attackPos = recentAttackPositions[Random.Range(0, recentAttackPositions.Count)];
            Vector3 newPosition = attackPos + (Vector3)(Random.insideUnitCircle.normalized * 1.5f);

            // Spawn teleport animation at the player's position
            if (teleportEffectPrefab != null)
            {
                Instantiate(teleportEffectPrefab, player.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(0.5f); // Wait for the animation to play

            // Move player after animation
            player.position = newPosition;

            // Spawn teleport animation at the new position
            if (teleportEffectPrefab != null)
            {
                Instantiate(teleportEffectPrefab, player.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(0.5f); // Small delay after teleport
        }
    }

    IEnumerator MeleeAttack()
    {
        animator.SetTrigger("Casting");
        yield return new WaitForSeconds(0.5f);

        if (Vector2.Distance(transform.position, player.position) <= meleeRange)
        {
            Instantiate(meleePrefab, player.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator SummonMonsters()
    {
        animator.SetTrigger("Casting");
        for (int i = 0; i < 4; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
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
        animator.SetBool("Phase1Idle", false);
        staff.SetActive(false);
        isPhaseTwo = true;
        bossCollider.enabled = true;
        attackRunning = false;
        StopAllCoroutines();
        animator.SetBool("IsIdle", true);
        StartCoroutine(PhaseTwoAttackPattern());
    }
}