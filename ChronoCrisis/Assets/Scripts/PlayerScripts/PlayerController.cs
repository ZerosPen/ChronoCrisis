using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineImpulseManager.ImpulseEvent;

public class PlayerController : MonoBehaviour
{
    [Header("Status")]
    public float HitPoint = 100f;
    public int level = 1;
    public float AtkPoint = 10;
    public int pointInt = 10;
    public int pointAgi = 10;
    public int pointVit = 10;

    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private UIManager uiManager;
    public float movementSpeed = 5f;
    public float AttackSpeed = 2f;
    [SerializeField] private float RunMulti = 2f;
    public float damageATK = 5f;
    [SerializeField] private float powerDash = 15f;
    [SerializeField] private float coolDownDash = 1f;
    public float manaPoint = 50f;
    [SerializeField] private float manaPointRegen = 5f;
    public bool levelUp = false;
    public Transform spawnPlayer;
    public int maxDashes = 2; // Allows up to double dash
    public float dashDuration = 0.2f; // Duration of each dash
    private int dashCount = 0;
    public float magicPower = 50f;
    public int DefendPoint = 20;
    public float currHitPoint;
    public float currManaPoint;
    private string damageType = "basic";
    public int playerExp = 0;
    public int enemyKilled = 0;
    private float attackRange = 5f;
    public int coins = 0;
    private Vector2 directionDash;

    public bool hasKey = false;

    [Header("conditon&requimen")]
    private bool isHealing = false;
    [SerializeField]private bool isRestorMana = false;
    public bool isDead = false;
    private bool isDashing = false;
    private bool canDash = true;
    public bool ActiveSkill = false;
    public bool isSlowed = false;
    private int baseMilestone = 100;
    public int nextMilestone;
    private bool isTempo = false;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyLayer;
    private SkillManager skillManager;
    private SaveManager saveManager;
    [SerializeField] InventoryManager inventoryManager;


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
        transform.position =  spawnPlayer.transform.position;
        rb = GetComponent<Rigidbody2D>();
        skillManager = GetComponent<SkillManager>();
        saveManager = GetComponent<SaveManager>();
        currHitPoint = HitPoint;
        currManaPoint = manaPoint;

        if (rb == null)
        {
            Debug.LogError("The Rigidbody2D is missing!");
        }
        if (uiManager == null)
        {
            Debug.LogError("uiManager is NULL");
        }

        nextMilestone = baseMilestone;
        
    }

    void Update()
    {
       SaveManager.instance.Save();
       // if (dialogueUI.IsOpen) return;

        movement();
        attack();
        skillCastQ();
        skillCastE();
        skillCastR();
        skillCastT();
        usingItem1();
        usingItem2();
        usingItem3();


        uiManager.UpdateHealth(currHitPoint,HitPoint);
        uiManager.UpdateMana(currManaPoint, manaPoint);
        uiManager.UpdateStatus(level, HitPoint, manaPoint, damageATK, magicPower ,DefendPoint, pointVit, pointAgi, pointInt);

        SaveManager.instance.UpdatePlayerData();
        SaveManager.instance.Save();

        if (currManaPoint < manaPoint)
        {
            isRestorMana = true;
            RestoreStatus(manaPointRegen);
        }

        //interact
        if(Input.GetKeyDown(KeyCode.F))
        {
            Interactable?.Interact(this);
            
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Player hit the wall!");
        }
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
            inventoryManager.ChangeSelectedSlot(0);
            skillManager.HandleSkillSwitching(0);
        }
    }

    void skillCastE()
    {
        if (Input.GetKeyDown(Skill2))
        {
            ActiveSkill = true;
            inventoryManager.ChangeSelectedSlot(1);
            skillManager.HandleSkillSwitching(1);
        }
    }

    void skillCastR()
    {
        if (Input.GetKeyDown(Skill3))
        {
            ActiveSkill = true;
            inventoryManager.ChangeSelectedSlot(2);
            skillManager.HandleSkillSwitching(2);
        }
    }

    void skillCastT()
    {
        if (Input.GetKeyDown(Skill4))
        {
            ActiveSkill = true;
            inventoryManager.ChangeSelectedSlot(3);
            skillManager.HandleSkillSwitching(3);
        }
    }

    #endregion

    //interact (F)
    void interact(){

    }

    // UsingItem
    #region (UsingItem)
    void usingItem1()
    {
        if (Input.GetKeyDown(item1))
        {

        }
    }

    void usingItem2()
    {
        if (Input.GetKeyDown(item2))
        {

        }
    }

    void usingItem3()
    {
        if (Input.GetKeyDown(item3))
        {

        }
    }

    #endregion

    //recieved item
    public void recievedTempDef(int index){
        if(index == 0)
        {
            Debug.Log("Booster Active");
            isTempo = true;
            DefendPoint += 5;
            StartCoroutine(DeactivePowerUp(0));
        }
        if (index == 1)
        {
            Debug.Log("Booster Active");
            isTempo = true;
            movementSpeed += 5;
            StartCoroutine(DeactivePowerUp(1));
        }
    }

    public void reciveExp(int exp)
    {
        playerExp += exp;

        if(playerExp >= baseMilestone)
        {
            levelUp = true;
            /*
            if(button) up Int -> maxMP += 5, magicPower += 2.5
            if(button) up Agi -> movementSpd = movementSpeed + movementSpeed*Agi, atkSpd = atkSpd + atkSpd*Agi/200
            if(button) up VIT -> maxHP += 5, def ++;
             */
            baseMilestone += 15*(level - 1);
        }
    }

    public void RestoreStatus(float value)
    {
        isRestorMana = true;

        if (currManaPoint < manaPoint && isRestorMana)
        {
            currManaPoint += value * Time.deltaTime; // Multiplied instead of divided

            if (currManaPoint > manaPoint)
            {
                currManaPoint = manaPoint; // Clamp to max mana
            }

            Debug.Log("Mana Restored: " + currManaPoint);
        }

        isRestorMana = false; // Stops when full
    }

    IEnumerator DeactivePowerUp(int index)
    {
        yield return new WaitForSeconds(15f);
        if(index == 0) 
        {
            DefendPoint -= 5;
            Debug.Log("Booster Deactive");
            isTempo = false;
        }
        if (index == 1)
        {
            movementSpeed -= 5;
            Debug.Log("Booster Dective");
            isTempo = false;
        }
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
            isDead = true;
            gameObject.SetActive(false);
        }
    }

    public void resetPositionPlayer()
    {
        if (spawnPlayer != null)
        {
            transform.position = spawnPlayer.position;
        }
        else
        {
            Debug.LogError("Spawn position is missing!");
        }
        currHitPoint = HitPoint;
        currManaPoint = manaPoint;
        gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

