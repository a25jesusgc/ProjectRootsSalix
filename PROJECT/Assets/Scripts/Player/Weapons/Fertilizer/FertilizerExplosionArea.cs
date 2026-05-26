using UnityEngine;

public class FertilizerExplosionArea : BulletHit
{
    public override void HitEnemy(Collision2D collision)
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si ha chocado contra un enemigo, éste recibe daño
        if (collision.CompareTag("Enemy"))
        {
            // Trata de obtener su componente EnemyHealth para llamar a la función ReceiveDamage
            if (collision.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.ReceiveDamage(damage, damageType);
            }
        }
    }

}
