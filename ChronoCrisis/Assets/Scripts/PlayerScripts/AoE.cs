using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/AoE")]
public class AoE : Skill
{
    public LayerMask enemyLayer;
    public override void useSkill(GameObject Player)
    {
        PointRatatioAction pra = GameObject.FindObjectOfType<PointRatatioAction>();

        if(Player == null)
        {
            Debug.LogError("Player is null!");
            return;
        }

        PlayerController playerStats = Player.GetComponent<PlayerController>();
        if (playerStats == null)
        {
            Debug.LogError("PlayerController component not found on Player!");
            return;
        }

        if (pra == null)
        {
            Debug.LogError("PointRatatioAction not found!");
            return;
        }

        Vector3 castPosition = pra.GetCursorPosition();
        float magicDamage = damageDeal + (playerStats.magicPower / 100);

        // Detect enemies in the radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPosition, radiusAoE, enemyLayer);

        foreach (Collider2D enemys in hitEnemies)
        {
            // Apply damage if the enemy has a health component
            EnemyController enemyHealth = enemys.GetComponent<EnemyController>();
            if (enemyHealth != null)
            {
                Debug.Log($"Hit enemy: {enemys.name} - Applying {magicDamage} damage");

                enemyHealth.EnemyTakeDamage(damageType == "magic" ? magicDamage : damageDeal, damageType);
            }
            else
            {
                Debug.Log("EnemyController component not found on " + enemys.name);
            }
        }
    }
}
