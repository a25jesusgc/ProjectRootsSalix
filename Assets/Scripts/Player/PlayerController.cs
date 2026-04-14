using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float INTERACT_RANGE = 1f;
    private const float CROSSHAIR_RANGE = 2f;
    private const float DASH_CD = 0.5f;
    private const float DASH_SPEED = 20f;
    private const float HOOK_JUMP_SPEED = 20f;
    [SerializeField] private PlayerInteractor interactBox;
    [SerializeField] private Transform crosshair;
    [SerializeField] private float movSpeed;

    private PlayerInput playerInput;
    private Rigidbody2D rb;
    private PlayerWeaponController playerWeaponController;
    private FertilizerSelector fertilizerSelector;

    private Vector2 movement;
    private float speed;
    private Vector2 aim;
    private Animator anim;
    private bool isAttacking;
    private bool isDashing;
    private float dashCooldown;
    private bool isHookJumping;
    private Transform hookTarget;

    public Vector2 GetAimDirection => aim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        playerWeaponController = GetComponent<PlayerWeaponController>();
        fertilizerSelector = GetComponent<FertilizerSelector>();
        anim = GetComponent<Animator>();
        transform.position = PlayerData.GetInstance.GetRespawn;
        speed = movSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalUtils.pause) return;

        interactBox.transform.localPosition = (movement.magnitude > 0 ? movement : aim) * INTERACT_RANGE;
        crosshair.localPosition = aim * CROSSHAIR_RANGE;
        
        if(dashCooldown > 0) dashCooldown -= Time.deltaTime;

        // Observa los inputs ejecutados por el jugador
        CheckInputs();
        
        // Gestiona las animaciones
        ManageAnims();
    }

    void FixedUpdate() {
        if(GlobalUtils.pause)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        // Mueve al jugador, dependiendo de si se desplaza manualmente o está siendo desplazado con el gancho
        if (isHookJumping && hookTarget != null)
        {
            rb.linearVelocity = (hookTarget.position - transform.position).normalized * HOOK_JUMP_SPEED;
        }
        else
        {
            rb.linearVelocity = movement * (isDashing ? DASH_SPEED : speed);
        }
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
        if (!isDashing && !isHookJumping)
        {
            // MOVIMIENTO
            // Vector de movimiento del jugador
            movement = playerInput.actions["Move"].ReadValue<Vector2>();

            // DISPARO
            // Check de si se pulsa el botón para la acción de disparar
            if (playerInput.actions["Shoot"].IsPressed())
            {
                playerWeaponController.ShootCurrentWeapon();
                isAttacking = true;
            }

            // DASH
            // Check de si se pulsa el botón para la acción de dashear
            if (dashCooldown <= 0 && playerInput.actions["Dash"].triggered)
            {
                StartCoroutine(DashCoroutine());
                dashCooldown = DASH_CD;
            }

            // INTERACTUAR
            // Check de si se pulsa el botón para interactuar
            if (playerInput.actions["Interact"].triggered)
            {
                interactBox.Interact();
            }
        }

        // DETENER DISPARO
        if (playerInput.actions["Shoot"].WasReleasedThisFrame())
        {
            playerWeaponController.StopCurrentWeapon();
            isAttacking = false;
        }

        // ARMA ANTERIOR
        // Check de si se pulsa el botón para cambiar al arma anterior
        if (playerInput.actions["PreviousWeapon"].triggered)
        {
            playerWeaponController.SelectPreviousWeapon();
        }

        // ARMA SIGUIENTE
        // Check de si se pulsa el botón para cambiar a la siguiente arma
        if (playerInput.actions["NextWeapon"].triggered)
        {
            playerWeaponController.SelectNextWeapon();
        }

        // FERTILIZANTE ANTERIOR
        // Check de si se pulsa el botón para cambiar al fertilizante anterior
        if (playerInput.actions["PreviousFertilizer"].triggered)
        {
            fertilizerSelector.SelectPreviousFertilizer();
        }

        // FERTILIZANTE SIGUIENTE
        // Check de si se pulsa el botón para cambiar al siguiente fertilizante
        if (playerInput.actions["NextFertilizer"].triggered)
        {
            fertilizerSelector.SelectNextFertilizer();
        }
        
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(0.1f);
        isDashing = false;
    }

    private void ManageAnims()
    {
        anim.speed = GlobalUtils.pause ? 0 : 1;
        anim.SetFloat("mov_x", movement.magnitude > 0 ? movement.x : aim.x);
        anim.SetFloat("mov_y", movement.magnitude > 0 ? movement.y : aim.y);
        anim.SetFloat("aim_x", aim.x);
        anim.SetFloat("aim_y", aim.y);
        anim.SetFloat("is_moving", movement.magnitude);
        anim.SetBool("attack", isAttacking);
        anim.SetBool("dash", isDashing || isHookJumping);
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void ResetSpeed()
    {
        speed = movSpeed;
    }

    public void StartHookJump(Transform newTarget)
    {
        isHookJumping = true;
        hookTarget = newTarget;
    }

    public void StopHookJump()
    {
        isHookJumping = false;
        hookTarget = null;
    }
}
