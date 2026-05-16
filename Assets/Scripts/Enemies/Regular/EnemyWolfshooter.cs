using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyWolfShooter : EnemyController
{
    // Bool para ejecutar de nuevo el ataque
    private bool isAttacking;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed=10.0f;
    [SerializeField] private AudioSource shootSound;
    [SerializeField] private float attackDuration;



    void OnDisable()
    {
        isAttacking = false;
        DestroyEnemyProjectiles();
    }

    protected override void Attack()
    {
        if (player == null) return; //Si no hay jugador, no continua

        if (isDefeated) //Si es derrotado, detiene el ataque y no continua
        {
            rb.linearVelocity = Vector2.zero;
            StopAllCoroutines();
            return;
        }

        if (isAttacking)
        {
            // Si ya está atacando, se queda quieto mientras ataca y no continua
            rb.linearVelocity = Vector2.zero;
            return;
        }

        StartCoroutine(WolfShooterAttackCoroutine());
    }


    private IEnumerator WolfShooterAttackCoroutine()
    {
        isAttacking = true; // No puede iniciar ataque porque ya está atacando
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento mientras ataca
        float timer=0;

        anim.SetTrigger("attack");

        while (timer < attackDuration)
        {
            if (player != null)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                anim.SetFloat("mov_x", dir.x);
                anim.SetFloat("mov_y", dir.y);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        //Calcula la direccion del jugador respecto al firepoint
        Vector2 playerDirection = (player.position - firePoint.position).normalized;

        //Apunta al jugador rotando lo necesario
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        //Instancia la bala donde se encuentra el enemigo
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
        //Le da velocidad
        bullet.GetComponent<Rigidbody2D>().linearVelocity = playerDirection*bulletSpeed;

        // Le pasa el daño que debe hacer a bullet
        bullet.GetComponent<EnemyProjectile>().damage = Mathf.RoundToInt(enemyType.GetAttackDamage * GetDayCycleAttackMultiplier());

        enemyProjectiles.Add(bullet);

        shootSound.Play();

        //yield return new WaitForSeconds(attackDuration); // Duración del ataque

        // Vuelve a estado perseguir (lo cual volverá a ataque en caso de seguir en el rango), y puede volver a atacar
        currentState = EnemyState.Chasing;
        isAttacking = false;
    }

    private float GetDayCycleAttackMultiplier()
    {
        float multiplier = 1f;

        if (DayCycleManager.instance.IsDay)
        {
            return enemyType.GetDayAttackMultipler;
        }
        else if (DayCycleManager.instance.IsNight)
        {
            return enemyType.GetNightAttackMultipler;
        }

        return multiplier;
    }
}