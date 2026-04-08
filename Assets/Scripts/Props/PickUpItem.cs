using UnityEngine;

public abstract class PickUpItem : MonoBehaviour
{
    // Identificador del pickup para eliminarlo si el jugador ya lo obtuvo
    [SerializeField] private string id;

    void Awake()
    {
        // Si el jugador ya obtuvo ese objeto, elimina el gameobject
        if(PlayerData.GetInstance.WasItemPicked(id)) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Al entrar en contacto con el jugador, el jugador obtiene el objeto
        if (collision.CompareTag("Player"))
        {
            // Objeto obtenido
            PlayerData.GetInstance.PickItem(id);
            // Se ejecuta OnPickup
            OnPickup(collision);
            // Y se destruye
            Destroy(gameObject);
        }
    }

    // Función que determina el comportamiento del objeto al ser obtenido
    public abstract void OnPickup(Collider2D collision);

}
