using UnityEngine;

public class PlayerCannon : PlayerWeapon
{
    [SerializeField] private GameObject bullet;
    private const float SHOOT_CD = 0.25f;

    public override void Shoot()
    {
        // Si está recargándose, no dispara
        if(shootCooldown > 0) return;

        // Instancia el objeto de bala
        GameObject bulletObject = Instantiate(bullet, transform.position, Quaternion.LookRotation(Vector3.forward, playerController.GetAimDirection));
        // Se le asigna su movimiento
        Vector2 mov = playerController.GetAimDirection * bulletSpeed;
        bulletObject.GetComponent<Rigidbody2D>().linearVelocity = mov;

        // Tras disparar necesita recargarse
        shootCooldown = SHOOT_CD;
    }
}
