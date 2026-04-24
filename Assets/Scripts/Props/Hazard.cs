using UnityEngine;

public class Hazard : MonoBehaviour
{

    // Se abre el panel de viaje rápido
    [SerializeField] private int damage;

    // Punto de recuperación del jugador donde aparecerá después de chocar contra el obstáculo
    [SerializeField] private Transform safePos;

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDamaged(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        PlayerDamaged(collision);
    }


    // Si el jugador entra en contacto con el obstáculo, recibe daño y reaparece en el safePos
    // (a no ser que esté siendo transportado por el gancho)
    private void PlayerDamaged(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController playerController))
        {
            if (!playerController.isHookJumping)
            {
                if (collision.TryGetComponent(out PlayerHealthController playerHealth))
                {
                    playerHealth.TakeDamage(damage);
                    if(safePos != null) collision.transform.position = safePos.position;
                }
            }
        }
    }
}
