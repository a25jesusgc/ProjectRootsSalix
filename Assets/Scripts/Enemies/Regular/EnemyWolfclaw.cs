using System.Collections;
using UnityEngine;
public class EnemyWolfClaw : EnemyController
{
    // Bool para ejecutar de nuevo el ataque
    private bool canAttack = true;

    protected override void Attack()
    {
        if (player == null || !canAttack) return; //Si no hay jugador o no puede atacar, se corta

        StartCoroutine(WolfClawAttackCoroutine());
    }

    private IEnumerator WolfClawAttackCoroutine()
    {
        canAttack = false; // No puede iniciar ataque porque ya está atacando
        rb.linearVelocity = Vector2.zero; // Detiene el movimiento mientras ataca

        anim.SetTrigger("attack"); // Animación de ataque
        Vector3 playerDirection = (player.position - transform.position).normalized;
        attack.transform.localPosition = playerDirection * 1f;
        attack.transform.rotation = Quaternion.LookRotation(Vector3.forward, playerDirection);

        yield return new WaitForSeconds(0.65f); // Duración del ataque

        // Vuelve a estado perseguir (lo cual volverá a ataque en caso de seguir en el rango), y puede volver a atacar
        currentState = EnemyState.Chasing;
        canAttack = true;
    }
}