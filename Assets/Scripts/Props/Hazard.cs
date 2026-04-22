using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Transform safePos;

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDamaged(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        PlayerDamaged(collision);
    }

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
