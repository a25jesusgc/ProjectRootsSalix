using System.Collections.Generic;
using UnityEngine;

public class PlayerVine : PlayerWeapon
{

    [SerializeField] private PlayerHealthController playerHealthController;
    [SerializeField] private GameObject vinePrefab;
    [SerializeField] private AudioSource vineSound;
    [SerializeField] private AudioSource vineDamageSound;
    private EnemyHealth target;
    private GameObject vine;
    private VineProjectile vineProjectile;
    private const float DMG_CD = 0.25f;
    private float dmgCooldown;
    private const float SHOOT_CD = 0.25f;

    public override void Update()
    {
        base.Update();
        if(dmgCooldown > 0) dmgCooldown -= Time.deltaTime;
    }

    public override void Shoot()
    {
        if(shootCooldown > 0) return;
        if (vine == null)
        {
            Vector3 position = transform.position + new Vector3(playerController.GetAimDirection.x, playerController.GetAimDirection.y);

            // Instancia el objeto de viña
            vine = Instantiate(vinePrefab, position, Quaternion.LookRotation(Vector3.forward, playerController.GetAimDirection));

            // Se le asigna su movimiento
            vineProjectile = vine.GetComponent<VineProjectile>();
            vineProjectile.SetPlayerVine(this);
            vineProjectile.SetVelocity(playerController.GetAimDirection, bulletSpeed);

            vineSound.Play();

        }else
        {
            if(target != null)
            {
                if(dmgCooldown > 0) return;
                if (target.GetHealthPercentage <= 0)
                {
                    StopVine();
                    return;
                }

                // Le inflinge daño
                target.ReceiveDamage(3, DamageType.THORN);
                // Y se cura
                playerHealthController.Heal(2, true);
                
                // Cooldown para el daño
                dmgCooldown = DMG_CD;
            }
            
        }
    }

    public void StopVine()
    {
        target = null;
        vineDamageSound.Stop();

        vineProjectile.ReturnVine();

        playerController.StopHookJump();
    }

    public void SetTarget(EnemyHealth enemy)
    {
        target = enemy;
        if(enemy != null)
        {
            vineDamageSound.Play();
        }
        
    }

    public override void StopShoot()
    {
        StopVine();
    }

    public void SetShotCooldown()
    {
        shootCooldown = SHOOT_CD;
    }
}
