using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chasing
    }

    private Transform player;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private float idleMoveRadius = 2f; // Radio dentro del cual el enemigo se mueve aleatoriamente cuando está en estado Idle
    [SerializeField] private Enemy enemyType; // Tipo de enemigo
    [SerializeField] private float idleWaitTime = 2f; // Tiempo que el enemigo espera antes de generar una nueva posición objetivo en estado Idle


    private Vector2 idleTargetPosition; // Posición objetivo para el movimiento aleatorio en estado Idle
    private Vector2 direction; // Dirección de movimiento
    private bool hadIdleTarget = false; // Variable para controlar si el enemigo ya tiene una posición objetivo en estado Idle
    public float detectionRadius = 5f;
    public float alertRadius = 3f;

    private Rigidbody2D rb;
    private EnemyState currentState;
    private Animator anim;

    // Variable para controlar si el enemigo está alerta
    private bool isAlerted;
    // Tiempo que el enemigo permanece alerta después de que el jugador dispara cerca de él
    private float alertTime = 5f;
    // Temporizador para controlar cuánto tiempo el enemigo permanece alerta
    private float alertTimer;

    private float idleWaitTimer = 0f; // Temporizador para controlar cuánto tiempo el enemigo espera antes de generar una nueva posición objetivo en estado Idle
    private bool isWaiting = false; // Variable para controlar si el enemigo está esperando antes de generar una nueva posición objetivo en estado Idle
    private bool isDefeated = false; // Variable para controlar si el enemigo está derrotado

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDefeated) return;
        // Verificar si el jugador está dentro del radio de alerta
        if (isAlerted)
        {
            // Si el jugador está dentro del radio de alerta, el enemigo se mantiene alerta
            alertTimer += Time.deltaTime;
            if (alertTimer >= alertTime)
            {
                // Si el tiempo de alerta ha pasado, el enemigo deja de estar alerta
                isAlerted = false;
                alertTimer = 0f;
            }
        }
        // Controla las animaciones
        ManageAnims();
    }

    void FixedUpdate()
    {
        // Verificar la distancia al jugador
        if (isDefeated) return;

        if (player != null)
        {
            // Si el jugador está dentro del radio de alerta, el enemigo se mantiene alerta
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Si el jugador está dentro del radio de detección o el enemigo está alerta, cambia al estado de persecución
            if (distanceToPlayer <= detectionRadius || isAlerted)
            {
                currentState = EnemyState.Chasing;
            }
            else
            {
                currentState = EnemyState.Idle;
            }
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                // El enemigo se mueve aleatoriamente dentro de un radio alrededor de su posición inicial
                // esto solo ocurre si el enemigo no está esperando antes de generar una nueva posición objetivo
                if (isWaiting)
                {
                    // Si el enemigo está esperando, incrementa el temporizador de espera
                    idleWaitTimer += Time.fixedDeltaTime;
                    if (idleWaitTimer >= idleWaitTime)
                    {
                        // Si el tiempo de espera ha pasado, el enemigo deja de esperar 
                        // y puede generar una nueva posición objetivo
                        isWaiting = false;
                        idleWaitTimer = 0f;
                        hadIdleTarget = false; // Resetea la variable para generar una nueva posición objetivo
                    }
                    rb.linearVelocity = Vector2.zero; // Detiene el movimiento mientras espera
                    return;
                }

                // El enemigo se mueve aleatoriamente dentro de un radio alrededor de su posición inicial
                if (!hadIdleTarget)
                {
                    // Si el enemigo no tiene una posición objetivo, genera una nueva posición aleatoria dentro del radio de movimiento
                    idleTargetPosition = GetRandomIdlePosition();
                    hadIdleTarget = true;
                }
                // Calcula la dirección hacia la posición objetivo y mueve al enemigo hacia esa posición
                direction = idleTargetPosition - (Vector2)transform.position;

                // Si el enemigo está lo suficientemente cerca de la posición objetivo, deja de moverse y genera una nueva posición objetivo en el siguiente ciclo
                if (direction.magnitude < 0.1f)
                {
                    // Si el enemigo ha llegado a la posición objetivo, resetea la variable para generar una 
                    // nueva posición objetivo en el siguiente ciclo
                    isWaiting = true; // El enemigo entra en estado de espera antes de generar una nueva posición objetivo
                    rb.linearVelocity = Vector2.zero;
                }
                else
                {
                    // Mueve al enemigo hacia la posición objetivo a una velocidad constante
                    rb.linearVelocity = direction.normalized * enemyType.GetMoveSpeed;
                }

                break;
            case EnemyState.Chasing:
                if(player == null) return;
                // El enemigo se mueve hacia el jugador
                isWaiting = false;
                direction = (player.position - transform.position).normalized;
                rb.MovePosition(transform.position + (Vector3) direction * enemyType.GetMoveSpeed * Time.fixedDeltaTime);
                break;
        }
    }

    private void ManageAnims()
    {
        anim.SetBool("is_moving", !isWaiting);
        anim.SetFloat("mov_x", direction.x);
        anim.SetFloat("mov_y", direction.y);
    }

    public void AlertBullet(Transform bulletTransform)
    {
        // Si el jugador dispara cerca del enemigo, el enemigo se alerta
        float distance = Vector2.Distance(transform.position, bulletTransform.position);

        if (distance <= alertRadius)
        {
            isAlerted = true;
            alertTimer = 0f; // Reiniciar el temporizador de alerta
        }
    }

    public void DetectPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    // Función para recibir daño
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();
            player.TakeDamage(enemyType.GetBodyDamage);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();
            player.TakeDamage(enemyType.GetBodyDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    // Función para generar una posición aleatoria dentro de un radio alrededor de la posición del enemigo
    Vector2 GetRandomIdlePosition()
    {
        // Genera un vector aleatorio dentro de un círculo unitario, 
        // lo normaliza para obtener solo la dirección y luego lo escala por el radio de movimiento
        Vector2 randomOffset = Random.insideUnitCircle.normalized;
        return (Vector2)transform.position + randomOffset * idleMoveRadius;
    }

    public void SetDefeated()
    {
        isDefeated = true;
        rb.linearVelocity = Vector2.zero;
        hitbox.enabled = false;
        anim.SetTrigger("hurt");
    }
}
