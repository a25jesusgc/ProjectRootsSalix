using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float INTERACT_RANGE = 1f;
    private const float CROSSHAIR_RANGE = 2f;
    private const float DASH_CD = 0.5f;
    [SerializeField] private PlayerInteractor interactBox;
    [SerializeField] private Transform crosshair;
    [SerializeField] private float movSpeed;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private PlayerWeaponController playerWeaponController;

    private Vector2 movement;
    private Vector2 aim;

    private bool isDashing;

    private float dashCooldown;

    public Vector2 GetAimDirection => aim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalUtils.pause) return;

        interactBox.transform.localPosition = aim * INTERACT_RANGE;
        crosshair.localPosition = aim * CROSSHAIR_RANGE;
        
        if(dashCooldown > 0) dashCooldown -= Time.deltaTime;

        // Observa los inputs ejecutados por el jugador
        CheckInputs();
        
    }

    void FixedUpdate() {
        if(GlobalUtils.pause)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        // Mueve al jugador
        rb.linearVelocity = movement * movSpeed * (isDashing ? 4f : 1f);
    }

    void CheckInputs()
    {
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

        // Si no está dasheando, puede moverse / disparar / dashear
        if (!isDashing)
        {
            // Vector de movimiento del jugador
            movement = playerInput.actions["Move"].ReadValue<Vector2>();

            // Check de si se pulsa el botón para la acción de disparar
            if (playerInput.actions["Shoot"].IsPressed())
            {
                playerWeaponController.ShootCurrentWeapon();
            }

            // Check de si se pulsa el botón para la acción de dashear
            if (dashCooldown <= 0 && playerInput.actions["Dash"].triggered)
            {
                StartCoroutine(DashCoroutine());
                dashCooldown = DASH_CD;
            }
        }

        // Check de si se pulsa el botón para cambiar al arma anterior
        if (playerInput.actions["PreviousWeapon"].triggered)
        {
            playerWeaponController.SelectPreviousWeapon();
        }

        // Check de si se pulsa el botón para cambiar a la siguiente arma
        if (playerInput.actions["NextWeapon"].triggered)
        {
            playerWeaponController.SelectNextWeapon();
        }

        // Check de si se pulsa el botón para interactuar
        if (playerInput.actions["Interact"].triggered)
        {
            interactBox.Interact();
        }
        
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
    }
}
