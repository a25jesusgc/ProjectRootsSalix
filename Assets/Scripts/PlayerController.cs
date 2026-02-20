using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float CROSSHAIR_RANGE = 2f;
    [SerializeField] private Transform crosshair;
    [SerializeField] private float movSpeed;

    private PlayerInput playerInput;
    private Rigidbody2D rb;

    private Vector2 movement;
    private Vector2 aim;

    private bool isDashing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Observa los inputs ejecutados por el jugador
        CheckInputs();
        rb.linearVelocity = movement * movSpeed * (isDashing ? 4f : 1f);
    }

    void CheckInputs()
    {
        if(isDashing) return;
        // Vector de movimiento del jugador
        movement = playerInput.actions["Move"].ReadValue<Vector2>();

        // Vector que representa la ubicación del cursor en pantalla
        InputAction aimAction = playerInput.actions["Aim"];
        Vector2 aimInput = aimAction.ReadValue<Vector2>();
        // Vector que representa la ubicación del jugador en pantalla
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        // Vector de apuntado, obteniendo la dirección en pantalla del cursor respecto al jugador
        aim = (aimInput - playerScreenPos).normalized;
        Debug.Log(aim);

        crosshair.localPosition = aim * CROSSHAIR_RANGE;

        // Check de si se pulsa el botón para la acción de disparar
        if (playerInput.actions["Shoot"].triggered)
        {
            Debug.Log("SHOOT");
        }

        // Check de si se pulsa el botón para la acción de dashear
        if (playerInput.actions["Dash"].triggered)
        {
            StartCoroutine(DashCoroutine());
        }
        
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
    }
}
