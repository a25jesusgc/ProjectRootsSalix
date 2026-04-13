using UnityEngine;

public class PlayerCannon : PlayerWeapon
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioSource shootSound;

    public override void Shoot()
    {
        // Si está recargándose, no dispara
        if(shootCooldown > 0) return;

        Vector3 position = transform.position + new Vector3(playerController.GetAimDirection.x, playerController.GetAimDirection.y);

        // Instancia el objeto de bala
        GameObject bulletObject = Instantiate(bullet, position, Quaternion.LookRotation(Vector3.forward, playerController.GetAimDirection));
        // Se le asigna su movimiento
        Vector2 mov = playerController.GetAimDirection * bulletSpeed;
        bulletObject.GetComponent<Rigidbody2D>().linearVelocity = mov;
        bulletObject.GetComponent<BulletHit>().damage = PlayerWeaponConstants.CANNON_DAMAGE;

        shootSound.Play();

        // Tras disparar necesita recargarse
        shootCooldown = PlayerWeaponConstants.CANNON_CD;
    }

    public override void StopShoot()
    {
        
    }
}
