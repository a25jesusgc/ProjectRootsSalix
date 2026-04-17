using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage;
    public GameObject hitEffect;

    void OnCollisionEnter2D(Collision2D collision)
    {
        HitPlayer(collision);
    }

    public virtual void HitPlayer(Collision2D collision)
    {
        // Si ha chocado contra el jugador, éste recibe daño
        if (collision.collider.CompareTag("Player"))
        {
            // Trata de obtener la vida del jugador y aplicar el daño
            if (collision.gameObject.TryGetComponent(out PlayerHealthController player))
            {
                player.TakeDamage(damage);
            }
        }
        
        // Instancia el efecto de la bala cuando impacta
        if(hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);

        // Destruye la bala
        Destroy(gameObject);
    }
}
