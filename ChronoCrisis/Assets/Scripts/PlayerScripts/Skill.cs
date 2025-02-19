using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Skill")]
public abstract class Skill : ScriptableObject
{
    public string skillName;
    public string typeSkill;
    public float coolDown;
    public float radiusAoE;
    public float damageDeal;
    public float manaUse;
    public string damageType;

    public virtual void useSkill(GameObject player) { }
}