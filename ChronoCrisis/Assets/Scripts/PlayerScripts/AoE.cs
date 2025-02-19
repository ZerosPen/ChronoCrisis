using UnityEngine;

[CreateAssetMenu(menuName = "Skills/FireballSkill")]
public class AoE : Skill
{
    public override void useSkill(GameObject Player)
    {
        Debug.Log("Casting Fireball! Deals " + damageDeal + " damage.");
    }
}
