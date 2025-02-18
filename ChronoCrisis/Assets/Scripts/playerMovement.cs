using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float hitPoints = 100f;
    public float movementSpeed = 5f;
    public float RunMulti = 2f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float damageATK = 5f;

    private bool isHealing = false;

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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

