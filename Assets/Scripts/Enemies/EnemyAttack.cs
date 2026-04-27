using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Enemy enemyType;

    [SerializeField] private float multiplier = 1f;

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
                playerHealth.TakeDamage(Mathf.RoundToInt(enemyType.GetAttackDamage * multiplier * GetDayCycleAttackMultiplier()));
            }
        }
    }

    public void SetMultiplier(float value)
    {
        multiplier = value;
    }

    private float GetDayCycleAttackMultiplier()
    {
        float multiplier = 1f;

        if (DayCycleManager.instance.IsDay)
        {
            return enemyType.GetDayAttackMultipler;
        }
        else if (DayCycleManager.instance.IsNight)
        {
            return enemyType.GetNightAttackMultipler;
        }

        return multiplier;
    }
}
