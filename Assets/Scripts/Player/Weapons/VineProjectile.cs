using System.Collections.Generic;
using UnityEngine;

public class VineProjectile : MonoBehaviour
{
    // Referencia al PlayerVine
    public PlayerVine playerVine;

    // Referencia al objetivo para engancharse a él
    private Transform target;
    private Collider2D targetCollider;

    // Componentes esenciales
    private Rigidbody2D rb;
    private LineTargets lineTargets;
    [SerializeField] private AudioSource vineSound;

    // Velocidad y dirección de movimiento del proyectil
    private float speed;
    private Vector2 direction;

    // Bandera que determina si ya se agarró a un objetivo
    private bool attached;

    // Duración máxima de viaje sin encontrar un objetivo antes de volver de vuelta
    private const float MAX_TRAVEL_TIME = 0.25f;

    // Distancia máxima a la que puede permanecer agarrado a un objetivo antes de romperse
    private const float MAX_RANGE = 8f;

    // Velocidad a la que se moverá el jugador mientras el gancho esté activo
    private const float PLAYER_VINE_SPEED = 3f;

    // Variable que determina el tiempo que lleva de viaje
    private float travelTime;

    // Bandera que determina si agarró un objeto (para VineGrab y VineHook)
    public bool grabbed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineTargets = GetComponent<LineTargets>();
    }

    void Update()
    {
        // Aumentamos el tiempo de viaje mientras no se haya agarrado a algo
        if(!attached) travelTime += Time.deltaTime;
        
        // Si alcanza el máximo de tiempo de viaje, el gancho vuelve
        if(travelTime >= MAX_TRAVEL_TIME)
        {
            playerVine.StopVine();
        }
    }

    void FixedUpdate()
    {
        // Si se agarró a un objetivo...
        if (attached)
        {
            // Mientras el objetivo sea válido, se mantiene enganchado
            if (target != null)
            {
                // Si se sale de rango, se cancela
                if(Vector3.Distance(target.position, playerVine.transform.position) > MAX_RANGE)
                {
                    playerVine.StopVine();
                    return;
                }
                // Si el objetivo deja de tener hitbox, se cancela
                if (targetCollider != null && !targetCollider.enabled)
                {
                    playerVine.StopVine();
                    return;
                }
                transform.position = target.position;
                rb.linearVelocity = Vector2.zero;
            }
            else // Si el objetivo ya no es válido, vuelve hacia el jugador
            {
                // Calcula la dirección del jugador y se mueve hacia él
                direction = playerVine.transform.position - transform.position;
                rb.linearVelocity = direction.normalized * speed;

                // Si alcanza al jugador, se recoge el gancho, destruyendo el proyectil, 
                // entrando en cooldown y restableciendo la velocidad del jugador
                if (Vector3.Distance(playerVine.transform.position, transform.position) < 0.5)
                {
                    playerVine.SetShotCooldown();
                    playerVine.playerController.ResetSpeed();
                    Destroy(gameObject);
                }
            }
        }
    }

    // Establece la referencia de PlayerVine para este objeto
    public void SetPlayerVine(PlayerVine value)
    {
        // Obtiene la referencia
        playerVine = value;

        // Establece objetivos para la conexión visual del gancho
        lineTargets.SetTargets(new List<Transform>() {playerVine.transform, transform});

        // Relentiza al jugador hasta que el gancho sea recogido de vuelta
        playerVine.playerController.SetSpeed(PLAYER_VINE_SPEED);
    }

    // Establece la velocidad y dirección del projectil
    public void SetVelocity(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        rb.linearVelocity = direction.normalized * speed;
    }

    // Se agarra a un objetivo
    public void AttachTarget(Transform newTarget)
    {
        target = newTarget;
        if (target.TryGetComponent(out Collider2D collider))
        {
            targetCollider = collider;
        }
        else
        {
            targetCollider = null;
        }
        attached = true;
        grabbed = true;
        vineSound.Play();
    }

    // Se cancela el objetivo, ya sea por dejar de disparar, que el objetivo ya no sea válido...
    public void RemoveTarget()
    {
        target = null;
        targetCollider = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Si entra en contacto con un enemigo y aún no se agarró a nada
        if (collision.CompareTag("Enemy") && !attached)
        {
            // Establece el enemigo objetivo en el PlayerVine y se agarra a él
            if (collision.TryGetComponent(out EnemyHealth enemy))
            {
                playerVine.SetTarget(enemy);
                AttachTarget(collision.transform);
            }
        }
    }

    // Establece que ya agarró (o que ya no puede agarrar) y que el objetivo no es válido 
    // para hacer que el proyectil regrese al jugador
    public void ReturnVine()
    {
        attached = true;
        target = null;
    }
}
