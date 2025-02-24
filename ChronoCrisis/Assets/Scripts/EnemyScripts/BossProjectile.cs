using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float damage; 
    private PlayerController playerController;
    public LayerMask Playerlayer;

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void OnTriggerEnter2D(Collider2D collision)
{
    Debug.Log("Trigger detected with: " + collision.gameObject.name);

    if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
    {
        Debug.Log("Projectile triggered with Player!");

        playerController = collision.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.recievedDamage(damage);
            Debug.Log("Player received damage: " + damage);
        }
        else
        {
            Debug.Log("PlayerController script is missing on Player!");
        }

        Destroy(gameObject);
    }
}

}