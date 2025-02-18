using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineImpulseManager.ImpulseEvent;

public class playerMovement : MonoBehaviour
{
    [Header("Status")]
    public float HitPoint = 100f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float RunMulti = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float damageATK = 5f;
    [SerializeField] private float powerDash = 15f;
    [SerializeField] private float coolDownDash = 1f;
    private Vector2 directionDash;

    [Header("conditon & requimen")]
    //private bool isHealing = false;
    //private bool isDead = false;
    private bool isDashing = false;
    private bool canDash = true;

    private Rigidbody2D rb;

    [SerializeField] private LayerMask enemyLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("The Rigidbody2D is missing!");
        }
    }

    void Update()
    {
        movement();
        attack();
    }

    void movement()
    {
        float verticalInput = Input.GetKey(KeyCode.W) ? 1f :
                              Input.GetKey(KeyCode.S) ? -1f : 0f;

        float horizontalInput = Input.GetKey(KeyCode.D) ? 1f :
                                Input.GetKey(KeyCode.A) ? -1f : 0f;

        Vector2 directionMove = new Vector2(horizontalInput, verticalInput).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            rb.velocity = directionMove * movementSpeed * RunMulti;
        }
        else
        {
            rb.velocity = directionMove * movementSpeed;
        }
        if (Input.GetMouseButtonDown(1) && canDash)
        {
            isDashing = true;
            canDash = false;
            directionDash =  new Vector2(horizontalInput, verticalInput);
            if(directionDash == Vector2.zero)
            {
                directionDash = new Vector2(transform.localScale.x, transform.localScale.y);
            }
            StartCoroutine(CoolDownDash());
        }

        if(isDashing)
        {
            rb.velocity = directionDash.normalized * powerDash;
            return;
        }
    }

    private void attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            Debug.Log($"Enemies detected: {hitEnemies.Length}");

            foreach (Collider2D enemys in hitEnemies)
            {

                if (enemys.gameObject.CompareTag("Enemy"))
                {
                    EnemyController enemy = enemys.GetComponent<EnemyController>();
                    enemy.EnemyTakeDamage(damageATK);
                }
            }
        }
    }


    //skill casting
    void skillCastQ(){

    }

    void skillCastE(){

    }

    void skillCastR(){

    }

    void skillCastT(){

    }

    //interact (F)
    void interact(){

    }

    //recieved item
    void recievedPowerUp(){
        
    }

    private IEnumerator CoolDownDash()
    {
        yield return new WaitForSeconds(coolDownDash);
        isDashing = false;
        canDash = true;
    }
    public void recievedDamage(float damage)
    {
        HitPoint -= damage;
        
        if(HitPoint <= 0)
        {
            //isDead = true;
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

