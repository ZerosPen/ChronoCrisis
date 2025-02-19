using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/FireballSkill")]
public class AoE : Skill
{
    public LayerMask enemyLayer;
    public override void useSkill(GameObject Player)
    {
        PointRatatioAction pra = GameObject.FindObjectOfType<PointRatatioAction>();
        if (pra == null)
        {
            Debug.LogError("PointRatatioAction not found!");
            return;
        }

        Vector3 castPosition = pra.GetCursorPosition();
        Debug.Log("Casting Fireball at " + castPosition + " Deals " + damageDeal + " damage.");

        // Detect enemies in the radius
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(castPosition, radiusAoE, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Apply damage if the enemy has a health component
            EnemyController enemyHealth = enemy.GetComponent<EnemyController>();
            if (enemyHealth != null)
            {
                enemyHealth.EnemyTakeDamage(damageDeal);
            }
        }
    }
}
