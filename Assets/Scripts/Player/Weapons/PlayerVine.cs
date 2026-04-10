using System.Collections.Generic;
using UnityEngine;

public class PlayerVine : PlayerWeapon
{
    // Para controlar la vida del jugador
    [SerializeField] private PlayerHealthController playerHealthController;

    // Proyectil que se lanza y engancha
    [SerializeField] private GameObject vinePrefab;
    // Efectos de sonido
    [SerializeField] private AudioSource vineSound;
    [SerializeField] private AudioSource vineDamageSound;

    // Enemigo objetivo
    private EnemyHealth enemyTarget;
    // Referencia al proyectil instanciado
    private GameObject vine;
    // Referencia al VineProjectile del proyectil instanciado
    private VineProjectile vineProjectile;
    private const float DMG_CD = 0.25f;
    private float dmgCooldown;
    private const float SHOOT_CD = 0.25f;

    // Se reducen los cooldowns
    public override void Update()
    {
        base.Update();
        if(dmgCooldown > 0) dmgCooldown -= Time.deltaTime;
    }

    public override void Shoot()
    {
        // Si el arma está en cooldown no dispara
        if(shootCooldown > 0) return;

        // Si es la primera llamada para disparar, crea el proyectil
        if (vine == null)
        {
            // Determina la posicion
            Vector3 position = transform.position + new Vector3(playerController.GetAimDirection.x, playerController.GetAimDirection.y);

            // Instancia el objeto de proyectil
            vine = Instantiate(vinePrefab, position, Quaternion.LookRotation(Vector3.forward, playerController.GetAimDirection));

            // Se le asigna su movimiento, y se configura su componente VineProjectile 
            // dándole una referencia a esta clase y estableciendo su velocidad y dirección inicial
            vineProjectile = vine.GetComponent<VineProjectile>();
            vineProjectile.SetPlayerVine(this);
            vineProjectile.SetVelocity(playerController.GetAimDirection, bulletSpeed);
            vineSound.Play();

        }
        else // Si ya existe el proyectil, gestionamos que haga daño al enemigo
        {
            // Si ha alcanzado a un enemigo y está enganchado a él...
            if(enemyTarget != null)
            {
                // Comprueba el cooldown para inflingir daño
                if(dmgCooldown > 0) return;
                
                // Comprueba también si el enemigo ya fue derrotado
                if (enemyTarget.GetHealthPercentage <= 0)
                {
                    StopVine();
                    return;
                }

                // En caso de estar agarrado a un enemigo vivo, le hace daño y drena vida
                // Le inflinge daño
                enemyTarget.ReceiveDamage(3, DamageType.THORN);
                // Y se cura
                playerHealthController.Heal(2, true);
                
                // Se resetea el cooldown para el daño
                dmgCooldown = DMG_CD;
            }
            
        }
    }

    // Función para hacer que el gancho vaya de vuelta
    public void StopVine()
    {
        // Elimina la referencia al enemigo objetivo en caso de tenerla
        enemyTarget = null;
        vineDamageSound.Stop();

        // Llama el proyectil de vuelta hacia el jugador
        vineProjectile.ReturnVine();

        // Y detiene el salto con gancho del jugador en caso de que estuviese en proceso
        playerController.StopHookJump();
    }

    // Función que establece el enemigo objetivo
    public void SetTarget(EnemyHealth enemy)
    {
        enemyTarget = enemy;
        if(enemy != null)
        {
            vineDamageSound.Play();
        }
        
    }

    // Si se deja de disparar, se llama a la función de vuelta del gancho
    public override void StopShoot()
    {
        StopVine();
    }

    public void SetShotCooldown()
    {
        shootCooldown = SHOOT_CD;
    }
}
