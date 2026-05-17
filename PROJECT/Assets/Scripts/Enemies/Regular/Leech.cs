using System.Collections;
using UnityEngine;

public class Leech : EnemyController
{
    // Bool para ejecutar de nuevo el ataque
    private bool isAttacking = false;

    private float LEECH_DASH_SPEED = 16f;

    void OnDisable()
    {
        isAttacking = false;
    }

    protected override void Attack()
    {
        if (player == null) return; //Si no hay jugador, no continua
        if (isAttacking)
        {
            return;
        }

        StartCoroutine(LeechAttackCoroutine());
    }

    private IEnumerator LeechAttackCoroutine()
    {
        isAttacking = true; // No puede iniciar ataque porque ya está atacando
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento en preparación
        anim.SetTrigger("attack"); // Animación de ataque
        Vector3 playerDirection = (player.position - transform.position).normalized;
        direction = playerDirection;

        yield return new WaitForSeconds(0.25f);

        // Se abalanza contra el jugador
        rb.linearVelocity = playerDirection * LEECH_DASH_SPEED;

        float timer = 1f;
        while (rb.linearVelocity != Vector2.zero && timer > 0 && !isDefeated)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f);

        // Vuelve a estado perseguir (lo cual volverá a ataque en caso de seguir en el rango), y puede volver a atacar
        currentState = EnemyState.Chasing;
        isAttacking = false;
    }
}
