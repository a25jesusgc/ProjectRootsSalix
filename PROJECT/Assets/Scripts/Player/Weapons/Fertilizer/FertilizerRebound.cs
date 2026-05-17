using UnityEngine;

public class FertilizerRebound : BulletHit
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private  float duration = 0.5f;
    [SerializeField] private  float durationExtension = 0.25f;
    [SerializeField] private  int maxRebounds = 5;

    private Vector2 lastVelocity;

    private int rebounds;

    private float liveTime;

    void Update()
    {
        lastVelocity = rb.linearVelocity;
        if (liveTime < duration)
        {
            liveTime += Time.deltaTime;
        }
        if (liveTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    public override void HitEnemy(Collision2D collision)
    {
        // Si ha chocado contra un enemigo, éste recibe daño
        if (collision.collider.CompareTag("Enemy"))
        {
            // Trata de obtener su componente EnemyHealth para llamar a la función ReceiveDamage
            if (collision.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.ReceiveDamage(damage, damageType);
            }
        
            // Instancia el efecto de la bala cuando impacta
            if(hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
        // Rebota
        Rebound(collision.GetContact(0).normal);
    }

    private void Rebound(Vector2 normal)
    {
        rebounds++;
        if(rebounds >= maxRebounds)
        {
            Destroy(gameObject);
            return;
        } 
        rb.linearVelocity = Vector2.Reflect(lastVelocity, normal);
        rb.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.linearVelocity.normalized);
        liveTime -= durationExtension;
    }
    
}
