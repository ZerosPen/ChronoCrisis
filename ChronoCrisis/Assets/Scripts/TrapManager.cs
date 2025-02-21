using System.Collections;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    public float damageDeal = 5f;
    private string damageType = "Trap";
    [SerializeField] private float coolDownTrap = 1.5f;
    [SerializeField] private float delay = 1.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                StartCoroutine(DelayDealingDamage(() => player.recievedDamage(damageDeal)));
            }
        }
        else if (collision.CompareTag("EnemyPhysical") || collision.CompareTag("EnemyMagic"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                StartCoroutine(DelayDealingDamage(() => enemy.EnemyTakeDamage(damageDeal, damageType)));
            }
        }
    }

    IEnumerator DelayDealingDamage(System.Action applyDamage)
    {
        yield return new WaitForSeconds(delay);
        applyDamage?.Invoke();
        StartCoroutine(CoolDownTrap());
    }

    IEnumerator CoolDownTrap()
    {
        yield return new WaitForSeconds(coolDownTrap);
    }   
}
