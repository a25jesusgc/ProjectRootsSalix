using UnityEngine;

public class LifeUpgrade : PickUpItem
{
    public override void OnPickup(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealthController playerHealthController))
        {
            playerHealthController.LifeUpgrade();
            Destroy(gameObject);
        }
    }
}
