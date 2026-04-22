using UnityEngine;

public class FertilizerPierce : BulletHit
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
        
                // Instancia el efecto de la bala cuando impacta
                if(hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
        }

        // Si choca contra pared o algo que no es enemigo, muere
        if (collision.CompareTag("Default"))
        {
            Destroy(gameObject);

            // Instancia el efecto de la bala cuando impacta
            if(hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }
}
