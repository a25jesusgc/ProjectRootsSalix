using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    private Interactable interactable;

    public void Interact()
    {
        if (interactable != null)
        {
            interactable.OnInteract();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            interactable = collision.GetComponent<Interactable>();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            interactable = null;
        }
    }
}
