using UnityEngine;

public abstract class PickUpItem : MonoBehaviour
{
    [SerializeField] private string id;

    void Awake()
    {
        if(PlayerData.GetInstance.WasItemPicked(id)) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerData.GetInstance.PickItem(id);
            OnPickup(collision);
        }
    }

    public abstract void OnPickup(Collider2D collision);

}
