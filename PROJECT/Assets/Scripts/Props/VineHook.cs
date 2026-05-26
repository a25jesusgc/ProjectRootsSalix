using UnityEngine;

public class VineHook : MonoBehaviour
{
    // Referencia al jugador y al gancho
    private PlayerController player;
    private VineProjectile vine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vine"))
        {
            if (collision.TryGetComponent(out VineProjectile vineProjectile))
            {
                // Si detecta un gancho, el gancho se agarra a este objeto
                vine = vineProjectile;
                if (!vine.grabbed)
                {
                    // El gancho se queda agarrado a este objeto y no puede agarrarse a otros
                    vine.grabbed = true;
                    vine.AttachTarget(transform);
                    // El jugador es desplazado en dirección al objeto
                    player = vineProjectile.playerVine.playerController;
                    player.StartHookJump(transform);
                }
            }
        }
        if (collision.CompareTag("Player"))
        {
            if(player != null)
            {
                // Si el jugador choca contra el objeto, ha llegado al objetivo y detiene el movimiento
                TargetReached();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(player != null)
            {
                // Si el jugador choca contra el objeto, ha llegado al objetivo y detiene el movimiento
                TargetReached();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(player != null)
            {
                // Si el jugador choca contra el objeto, ha llegado al objetivo y detiene el movimiento
                TargetReached();
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(player != null)
            {
                // Si el jugador choca contra el objeto, ha llegado al objetivo y detiene el movimiento
                TargetReached();
            }
        }
    }

    // Si el jugador choca contra el objeto, ha llegado al objetivo y detiene el movimiento
    // Y el gancho regresa cumpliendo su función
    private void TargetReached()
    {
        player.StopHookJump();
        player = null;
        vine.playerVine.StopVine();
    }
}
