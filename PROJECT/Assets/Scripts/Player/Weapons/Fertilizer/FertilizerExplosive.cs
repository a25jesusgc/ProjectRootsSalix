using UnityEngine;

public class FertilizerExplosive : BulletHit
{
    public override void HitEnemy(Collision2D collision)
    {
        // Instancia el efecto de la explosión
        if(hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
