using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    private const float CROSSHAIR_RANGE = 2f;
    private const float SHOOT_CD = 0.25f;
    private const float DASH_CD = 0.5f;
    [SerializeField] private Transform crosshair;
    [SerializeField] private float movSpeed;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private PlayerShoot playerShoot;

    private Vector2 movement;
    private Vector2 aim;

    private bool isDashing;

    private float dashCooldown;
    private float shootCooldown;

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
        crosshair.localPosition = aim * CROSSHAIR_RANGE;

        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
        if (shootCooldown > 0) shootCooldown -= Time.deltaTime;

        // Observa los inputs ejecutados por el jugador
        CheckInputs();
    }

    void FixedUpdate()
    {
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
            }
            else
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
            if (shootCooldown <= 0 && playerInput.actions["Shoot"].IsPressed())
            {
                playerShoot.Shoot();
                shootCooldown = SHOOT_CD;
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
            // TODO implement weapon system
        }

        // Check de si se pulsa el botón para cambiar a la siguiente arma
        if (playerInput.actions["NextWeapon"].triggered)
        {
            // TODO implement weapon system
        }

    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
    }

}
