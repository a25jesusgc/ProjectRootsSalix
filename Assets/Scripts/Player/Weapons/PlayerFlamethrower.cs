using UnityEngine;

public class PlayerFlamethrower : PlayerWeapon
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private PlayerHealthController playerHealthController;
    [SerializeField] private AudioSource shootSound;
    private const float SHOOT_CD = 0.05f;
    private const float ANGLE_SPREAD = 8f;

    public override void Shoot()
    {
        // Si está recargándose, no dispara
        if(shootCooldown > 0) return;
        
        // Disparar consume vida
        playerHealthController.TakeDamage(1, true);

        // Calcula la dirección
        Vector2 direction = Quaternion.Euler(0f, 0f, Random.Range(-ANGLE_SPREAD, ANGLE_SPREAD)) * playerController.GetAimDirection;
        Vector3 position = transform.position + new Vector3(direction.x, direction.y);

        // Instancia el objeto de llama
        GameObject bulletObject = Instantiate(bullet, position, Quaternion.LookRotation(Vector3.forward, direction));
        // Se le asigna su movimiento
        Vector2 mov = direction * bulletSpeed;
        bulletObject.GetComponent<Rigidbody2D>().linearVelocity = mov;

        if(!shootSound.isPlaying) shootSound.Play();

        // Tras disparar necesita recargarse
        shootCooldown = SHOOT_CD;
    }

    public override void StopShoot()
    {
        shootSound.Stop();
    }
}
