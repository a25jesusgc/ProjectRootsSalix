using UnityEngine;

// Componente que permite el cambio de zona
public class ZoneChanger : MonoBehaviour
{
    // Escena a la que pertenece y que va a ser abandonada
    [SerializeField] private string unload;

    // Escena a la que va y será cargada
    [SerializeField] private string load;

    // Posición objetivo a la que se llevará al jugador
    [SerializeField] private Transform target;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Si el jugador entra en el cambiador de zona, realiza el cambio de zona usando ZoneLoader
            ZoneLoader.instance.ChangeZone(unload, load, target.position);
        }
    }
}
