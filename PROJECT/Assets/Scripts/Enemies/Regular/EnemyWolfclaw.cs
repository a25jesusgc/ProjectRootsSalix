using System.Collections;
using UnityEngine;
public class EnemyWolfClaw : EnemyController
{
    // Bool para ejecutar de nuevo el ataque
    private bool isAttacking = false;

    void OnDisable()
    {
        isAttacking = false;
        attack.SetActive(false);
    }
    
    protected override void Attack()
    {
        if (player == null) return; //Si no hay jugador, no continua
        if (isAttacking)
        {
            // Si ya está atacando, se queda quieto mientras ataca y no continua
            rb.linearVelocity = Vector2.zero;
            return;
        }

        StartCoroutine(WolfClawAttackCoroutine());
    }

    private IEnumerator WolfClawAttackCoroutine()
    {
        isAttacking = true; // No puede iniciar ataque porque ya está atacando
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento mientras ataca

        anim.SetTrigger("attack"); // Animación de ataque
        Vector3 playerDirection = (player.position - transform.position).normalized;
        attack.transform.localPosition = playerDirection * 1f;
        attack.transform.rotation = Quaternion.LookRotation(Vector3.forward, playerDirection);

        yield return new WaitForSeconds(0.65f); // Duración del ataque

        // Vuelve a estado perseguir (lo cual volverá a ataque en caso de seguir en el rango), y puede volver a atacar
        currentState = EnemyState.Chasing;
        isAttacking = false;
    }
}