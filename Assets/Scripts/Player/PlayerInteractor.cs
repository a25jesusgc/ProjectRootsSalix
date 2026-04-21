using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private Interactable interactable;

    public void Interact()
    {
        if (interactable != null)
        {
            interactable.OnInteract(transform.parent);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (collision.TryGetComponent(out Interactable interactable))
            {
                this.interactable = interactable;
                interactable.ShowInteractionIcon(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            interactable.ShowInteractionIcon(false);
            interactable = null;
        }
    }
}
