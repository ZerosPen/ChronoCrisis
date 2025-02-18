using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineImpulseManager.ImpulseEvent;

public class PlayerController : MonoBehaviour
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

    [Header("conditon&requimen")]
    //private bool isHealing = false;
    //private bool isDead = false;
    private bool isDashing = false;
    private bool canDash = true;
    public bool ActiveSkill = false;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject aoeIndicatorPrefab; // Drag an AoE preview prefab
    private GameObject aoeIndicatorInstance;


    [Header("KeyBlind")]
    public KeyCode moveLeft = KeyCode.A;
    public KeyCode moveRight = KeyCode.D;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode Skill1 = KeyCode.Q;
    public KeyCode Skill2 = KeyCode.E;
    public KeyCode Skill3 = KeyCode.R;
    public KeyCode Skill4 = KeyCode.T;
    public KeyCode item1 = KeyCode.Alpha1;
    public KeyCode item2 = KeyCode.Alpha2;
    public KeyCode item3 = KeyCode.Alpha3;


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
        skillCastQ();
    }

    void movement()
    {
        float verticalInput = Input.GetKey(moveUp) ? 1f :
                              Input.GetKey(moveDown) ? -1f : 0f;

        float horizontalInput = Input.GetKey(moveRight) ? 1f :
                                Input.GetKey(moveLeft) ? -1f : 0f;

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
        if (Input.GetMouseButtonDown(0) && ActiveSkill != true)
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
    #region (skill casting)
    void skillCastQ()
    {
        if (Input.GetKeyDown(Skill1))
        {
            ActiveSkill = true;
            
            StartCoroutine(CoolDownChangeSkill());
        }
    }

    void skillCastE()
    {
        if (Input.GetKeyDown(Skill2))
        {
            ActiveSkill = true;
            
            StartCoroutine(CoolDownChangeSkill());
        }
    }

    void skillCastR()
    {
        if (Input.GetKeyDown(Skill3))
        {
            ActiveSkill = true;
            
            StartCoroutine(CoolDownChangeSkill());
        }
    }

    void skillCastT()
    {
        if (Input.GetKeyDown(Skill4))
        {
            ActiveSkill = true;
            
            StartCoroutine(CoolDownChangeSkill());
        }
    }
    #endregion

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

    private IEnumerator CoolDownChangeSkill()
    {
        yield return new WaitForSeconds(1f);
        ActiveSkill = false;
    }

    void StartAoE()
    {
        if (aoeIndicatorInstance == null && aoeIndicatorPrefab != null)
        {
            aoeIndicatorInstance = Instantiate(aoeIndicatorPrefab);
        }
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

