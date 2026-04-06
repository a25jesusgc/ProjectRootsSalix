using UnityEngine;

public class PlayerFlamethrower : PlayerWeapon
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private PlayerHealthController playerHealthController;
    private const float SHOOT_CD = 0.05f;
    private const float ANGLE_SPREAD = 15f;

    public override void Shoot()
    {
        // Si está recargándose, no dispara
        if(shootCooldown > 0) return;

        // Disparar consume vida
        playerHealthController.TakeDamage(1);

        // Calcula la dirección
        Vector2 direction = Quaternion.Euler(0f, 0f, Random.Range(-ANGLE_SPREAD, ANGLE_SPREAD)) * playerController.GetAimDirection;

        // Instancia el objeto de llama
        GameObject bulletObject = Instantiate(bullet, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        // Se le asigna su movimiento
        Vector2 mov = direction * bulletSpeed;
        bulletObject.GetComponent<Rigidbody2D>().linearVelocity = mov;

        // Tras disparar necesita recargarse
        shootCooldown = SHOOT_CD;
    }
}
