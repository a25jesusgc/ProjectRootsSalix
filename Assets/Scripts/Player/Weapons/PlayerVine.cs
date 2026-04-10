using System.Collections.Generic;
using UnityEngine;

public class PlayerVine : PlayerWeapon
{

    [SerializeField] private PlayerHealthController playerHealthController;
    [SerializeField] private GameObject vinePrefab;
    private EnemyHealth target;
    private GameObject vine;
    private VineProjectile vineProjectile;
    private float timer;
    private const float SHOOT_CD = 0.25f;
    private const float TIMER_TIME = 0.05f;

    public override void Update()
    {
        base.Update();
        if(timer > 0) timer -= Time.deltaTime;
        if (vine != null && timer <= 0)
        {
            target = null;
            vineProjectile.ReturnVine();
        }
    }

    public override void Shoot()
    {
        if (vine == null)
        {
            if(timer > 0) return;
            timer = TIMER_TIME;
            Vector3 position = transform.position + new Vector3(playerController.GetAimDirection.x, playerController.GetAimDirection.y);

            // Instancia el objeto de viña
            vine = Instantiate(vinePrefab, position, Quaternion.LookRotation(Vector3.forward, playerController.GetAimDirection));

            // Se le asigna su movimiento
            vineProjectile = vine.GetComponent<VineProjectile>();
            vineProjectile.SetPlayerVine(this);
            vineProjectile.SetVelocity(playerController.GetAimDirection, bulletSpeed);

        }else
        {
            timer = TIMER_TIME;
            if(target != null)
            {
                if(shootCooldown > 0) return;
                if (target.GetHealthPercentage <= 0)
                {
                    target = null;
                    vineProjectile.ReturnVine();
                    return;
                }

                // Le inflinge daño
                target.ReceiveDamage(3, DamageType.THORN);
                // Y se cura
                playerHealthController.Heal(2, true);
                
                // Cooldown para el daño
                shootCooldown = SHOOT_CD;
            }
            
        }
    }

    public void SetTarget(EnemyHealth enemy)
    {
        target = enemy;
    }
}
