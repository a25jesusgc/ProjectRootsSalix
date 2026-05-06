using UnityEngine;

public class LifeUpgrade : PickUpItem
{
    [SerializeField] private GameObject healEffect;

    // Al coger mejora de vida, súma la vida a los datos del jugador
    public override void OnPickup(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealthController playerHealthController))
        {
            Instantiate(healEffect, collision.transform.position, Quaternion.identity);
            playerHealthController.LifeUpgrade();
        }
    }
}
