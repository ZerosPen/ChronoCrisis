using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float HitPoint = 100;
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float MovementChase = 2.5f;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float stopDuration = 3f;

    [SerializeField] private Transform player;
    [SerializeField] private CapsuleCollider cc;
    private bool isFacingRight = true;
    private bool isChasing = false;
    private bool isStopped = false;
    [SerializeField] private Transform waypointsA;
    [SerializeField] private Transform waypointsB;
    private Vector2 targetWayPoint;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CapsuleCollider>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        SetNewTargetPos();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            
        if(distanceToPlayer <= chaseRange) 
        {
            isChasing = true;
            chaseTarget();
        }
        else
        {
            isChasing = false;

            if (!isStopped)
            {
                MoveToWardTarget();
            }
        }
    }

    void SetNewTargetPos()
    {
        //set new waypoint
        float randomX = Random.Range(waypointsA.position.x, waypointsB.position.x);
        float randomY = Random.Range(waypointsA.position.y, waypointsB.position.y);

        targetWayPoint = new Vector2(randomX, randomY);
    }

    private void MoveToWardTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetWayPoint, MovementSpeed * Time.deltaTime);
        
        if(Vector2.Distance(transform.position, targetWayPoint) <= 0.1f)
        {
            isStopped = true;
            Invoke("ResumePatrol", stopDuration);
        }
    }

    private void chaseTarget()
    {
        if(isChasing == true)
        {
            //Vector2 direction = (player.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, player.position, MovementSpeed * MovementChase * Time.deltaTime);
        }
    }

    private void ResumePatrol()
    {
        SetNewTargetPos();
        isStopped = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

/*        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);*/
    }

}
