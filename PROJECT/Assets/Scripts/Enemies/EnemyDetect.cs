using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            enemyController.AlertBullet(collision.transform);
        }

        // Si el jugador se acerca a su rango de visión, lo detecta
        if (collision.CompareTag("Player"))
        {
            enemyController.DetectPlayer(collision.transform);
        }
    }
}
