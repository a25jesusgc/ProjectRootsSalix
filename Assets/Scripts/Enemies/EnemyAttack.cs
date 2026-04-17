using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy enemyType;

    void OnTriggerEnter2D(Collider2D collision)
    {
        DamagePlayer(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        DamagePlayer(collision);
    }

    private void DamagePlayer(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerHealthController playerHealth))
            {
                playerHealth.TakeDamage(enemyType.GetAttackDamage);
            }
        }
    }
}
