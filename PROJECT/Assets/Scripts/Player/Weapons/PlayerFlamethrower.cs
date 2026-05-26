using UnityEngine;

public class PlayerFlamethrower : PlayerWeapon
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private PlayerHealthController playerHealthController;
    [SerializeField] private AudioSource shootSound;

    public override void Shoot()
    {
        // Si está recargándose, no dispara
        if(shootCooldown > 0) return;
        
        // Disparar consume vida
        playerHealthController.TakeDamage(PlayerWeaponConstants.FLAMETHROWER_SELF_DAMAGE, true);

        // Calcula la dirección
        Vector2 direction = Quaternion.Euler(0f, 0f, Random.Range(-PlayerWeaponConstants.FLAMETHROWER_ANGLE_SPREAD, PlayerWeaponConstants.FLAMETHROWER_ANGLE_SPREAD)) * playerController.GetAimDirection;
        Vector3 position = transform.position + new Vector3(direction.x, direction.y);

        // Instancia el objeto de llama
        GameObject bulletObject = Instantiate(bullet, position, Quaternion.LookRotation(Vector3.forward, direction));
        // Se le asigna su movimiento
        Vector2 mov = direction * bulletSpeed;
        bulletObject.GetComponent<Rigidbody2D>().linearVelocity = mov;
        bulletObject.GetComponent<BulletHit>().damage = PlayerWeaponConstants.FLAMETHROWER_DAMAGE;

        if(!shootSound.isPlaying) shootSound.Play();

        // Tras disparar necesita recargarse
        shootCooldown = PlayerWeaponConstants.FLAMETHROWER_CD;
    }

    public override void StopShoot()
    {
        shootSound.Stop();
    }
}
