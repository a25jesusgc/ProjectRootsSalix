using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;

    private Vector2 movement;
    private Vector2 aim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        // Observa los inputs ejecutados por el jugador
        CheckInputs();
    }

    void CheckInputs()
    {
        // Vector de movimiento del jugador
        movement = playerInput.actions["Move"].ReadValue<Vector2>();

        // Vector que representa la ubicación del cursor en pantalla
        Vector2 aimInput = playerInput.actions["Aim"].ReadValue<Vector2>();
        // Vector que representa la ubicación del jugador en pantalla
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        // Vector de apuntado, obteniendo la dirección en pantalla del cursor respecto al jugador
        aim = (aimInput - playerScreenPos).normalized;
        Debug.Log(aim);

        // Check de si se pulsa el botón para la acción de disparar
        if (playerInput.actions["Shoot"].triggered)
        {
            Debug.Log("SHOOT");
        }

        // Check de si se pulsa el botón para la acción de dashear
        if (playerInput.actions["Dash"].triggered)
        {
            Debug.Log("DASH");
        }
        
    }
}
