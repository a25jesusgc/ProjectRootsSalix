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
    private PlayerShoot playerShoot;

    private Vector2 movement;
    private Vector2 aim;

    private bool isDashing;

    public Vector2 GetAimDirection => aim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        playerShoot = GetComponent<PlayerShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        // Observa los inputs ejecutados por el jugador
        CheckInputs();
        rb.linearVelocity = movement * movSpeed * (isDashing ? 4f : 1f);
        crosshair.localPosition = aim * CROSSHAIR_RANGE;
    }

    void CheckInputs()
    {
        if(isDashing) return;
        // Vector de movimiento del jugador
        movement = playerInput.actions["Move"].ReadValue<Vector2>();

        // Vector que representa la ubicación del cursor en pantalla
        InputAction aimAction = playerInput.actions["Aim"];
        Vector2 aimInput = aimAction.ReadValue<Vector2>();
        if (aimAction.activeControl != null)
        {
            bool isGamepad = aimAction.activeControl.device is Gamepad;
            if (isGamepad)
            {
                // Vector de apuntado usando el axis del joystick
                aim = aimInput.normalized;
            }else
            {
                // Vector que representa la ubicación del jugador en pantalla
                Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);
                // Vector de apuntado, obteniendo la dirección en pantalla del cursor respecto al jugador
                aim = (aimInput - playerScreenPos).normalized;
            }
        }

        // Check de si se pulsa el botón para la acción de disparar
        if (playerInput.actions["Shoot"].triggered)
        {
            playerShoot.Shoot();
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
