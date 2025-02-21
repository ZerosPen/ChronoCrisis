using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineImpulseManager.ImpulseEvent;

public class PlayerController : MonoBehaviour
{
    [Header("Status")]
    public float HitPoint = 100f;

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float RunMulti = 2f;
    [SerializeField] private float damageATK = 5f;
    [SerializeField] private float powerDash = 15f;
    [SerializeField] private float coolDownDash = 1f;
    [SerializeField] private float manaPoint = 50f;
    [SerializeField] private float manaPointRegen = 5f;
    public float magicPower = 50f;
    private float Def = 20f;
    public float currHitPoint;
    public float currManaPoint;
    private string damageType = "basic";

    private float attackRange = 5f;
    private Vector2 directionDash;

    [Header("conditon&requimen")]
    private bool isHealing = false;
    private bool isRestorMana = false;
    private bool isDead = false;
    private bool isDashing = false;
    private bool canDash = true;
    public bool ActiveSkill = false;
    public bool isSlowed = false;   
    private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyLayer;
    private SkillManager skillManager;


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

    public DialogueUI DialogueUI => dialogueUI;

    public IInteractable Interactable {get; set;}


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skillManager = GetComponent<SkillManager>();
        currHitPoint = HitPoint;
        currManaPoint = manaPoint;

        if (rb == null)
        {
            Debug.LogError("The Rigidbody2D is missing!");
        }
    }

    void Update()
    {
       // if (dialogueUI.IsOpen) return;

        movement();
        attack();
        skillCastQ();
        skillCastE();
        skillCastR();
        skillCastT();
        if (currManaPoint < manaPoint)
        {
            isRestorMana = true;
            RestoreMana();
        }

        //interact
        if(Input.GetKeyDown(KeyCode.F))
        {
            Interactable?.Interact(this);
            
        } 



    }


    private int dashCount = 0;
    public int maxDashes = 2; // Allows up to double dash
    public float dashDuration = 0.2f; // Duration of each dash

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
        
        //edit : enabled double dash
        if (Input.GetMouseButtonDown(1) && canDash && dashCount < maxDashes)
        {
            isDashing = true;
            dashCount++;
            directionDash =  new Vector2(horizontalInput, verticalInput);
            if(directionDash == Vector2.zero)
            {
                directionDash = new Vector2(transform.localScale.x, transform.localScale.y);
            }
            StartCoroutine(DashDuration());

            //double dash mechanic
            if (dashCount == 1 || dashCount == maxDashes)
            {
                StartCoroutine(CoolDownDash());
                if (dashCount == maxDashes)
                {
                    canDash = false;
                }
            }

        }

        if(isDashing)
        {
            rb.velocity = directionDash.normalized * powerDash;
            return;
        }
    }

    //dash duration
    IEnumerator DashDuration()
    {
        yield return new WaitForSeconds(dashDuration); // Set your desired dash duration
        isDashing = false;
        rb.velocity = Vector2.zero; // Stop movement after dashing
    }

    private void attack()
    {
        if (Input.GetMouseButtonDown(0) && ActiveSkill != true)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
            Debug.Log($"Enemies detected: {hitEnemies.Length}");

            foreach (Collider2D enemys in hitEnemies)
            {

                if (enemys.gameObject.CompareTag("EnemyPhysical") || (enemys.gameObject.CompareTag("EnemyMagic")))
                {
                    EnemyController enemy = enemys.GetComponent<EnemyController>();
                    enemy.EnemyTakeDamage(damageATK, damageType);
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
            skillManager.HandleSkillSwitching(0);
        }
    }

    void skillCastE()
    {
        if (Input.GetKeyDown(Skill2))
        {
            ActiveSkill = true;
            skillManager.HandleSkillSwitching(1);
        }
    }

    void skillCastR()
    {
        if (Input.GetKeyDown(Skill3))
        {
            ActiveSkill = true;
            skillManager.HandleSkillSwitching(2);
        }
    }

    void skillCastT()
    {
        if (Input.GetKeyDown(Skill4))
        {
            ActiveSkill = true;
            skillManager.HandleSkillSwitching(3);
        }
    }

    #endregion

    //interact (F)
    void interact(){

    }

    //recieved item
    void recievedPowerUp(){
        
    }

    void RestoreMana()
    {
        isRestorMana = true;

        if (currManaPoint < manaPoint && isRestorMana)
        {

            currManaPoint += manaPointRegen / Time.deltaTime;
            
            Debug.Log("Mana Restored: " + currManaPoint);
        }

        isRestorMana = false; // Stops when full
    }


    private IEnumerator CoolDownDash()
    {
        yield return new WaitForSeconds(coolDownDash);
        dashCount = 0;
        isDashing = false;
        canDash = true;
    }

    public IEnumerator CoolDownChangeSkill()
    {
        yield return new WaitForSeconds(1f);
        ActiveSkill = false;
    }

    public void recievedDamage(float damage)
    {
        currHitPoint -= damage;
        
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

