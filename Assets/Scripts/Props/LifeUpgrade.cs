using UnityEngine;

public class LifeUpgrade : PickUpItem
{
    // Al coger mejora de vida, súma la vida a los datos del jugador
    public override void OnPickup(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealthController playerHealthController))
        {
            playerHealthController.LifeUpgrade();
        }
    }
}
