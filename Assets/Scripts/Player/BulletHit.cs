using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public int damage;
    public DamageType damageType;
    public GameObject hitEffect;

    void OnCollisionEnter2D(Collision2D collision)
    {
        HitEnemy(collision);
    }

    public virtual void HitEnemy(Collision2D collision)
    {
        // Si ha chocado contra un enemigo, éste recibe daño
        if (collision.collider.CompareTag("Enemy"))
        {
            // Trata de obtener su componente EnemyHealth para llamar a la función ReceiveDamage
            if (collision.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.ReceiveDamage(damage, damageType);
            }
        }
        
        // Instancia el efecto de la bala cuando impacta
        if(hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);

        // Destruye la bala
        Destroy(gameObject);
    }
}
