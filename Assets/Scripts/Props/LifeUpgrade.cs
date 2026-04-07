using UnityEngine;

public class LifeUpgrade : MonoBehaviour
{
    [SerializeField] private int amount;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerHealthController playerHealthController))
            {
                playerHealthController.LifeUpgrade(amount);
                Destroy(gameObject);
            }
        }
    }
}
