using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chasing,
        Attack
    }

    protected Transform player;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] private float idleMoveRadius = 2f; // Radio dentro del cual el enemigo se mueve aleatoriamente cuando está en estado Idle
    [SerializeField] protected Enemy enemyType; // Tipo de enemigo
    [SerializeField] private float idleWaitTime = 2f; // Tiempo que el enemigo espera antes de generar una nueva posición objetivo en estado Idle


    private Vector2 idleTargetPosition; // Posición objetivo para el movimiento aleatorio en estado Idle
    [HideInInspector] public Vector2 direction; // Dirección de movimiento
    private bool hadIdleTarget = false; // Variable para controlar si el enemigo ya tiene una posición objetivo en estado Idle
    public float detectionRadius = 5f;
    public GameObject attack;
    private Animator attackAnim;
    public float attackRange = 1f;

    // Proyectiles del enemigo para destruirlos cuando muere
    [HideInInspector] public List<GameObject> enemyProjectiles;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public EnemyState currentState;
    protected Animator anim;

    // Variable para controlar si el enemigo está alerta
    private bool isAlerted;
    // Tiempo que el enemigo permanece alerta después de que el jugador dispara cerca de él
    private float alertTime = 5f;
    // Temporizador para controlar cuánto tiempo el enemigo permanece alerta
    private float alertTimer;

    private float idleWaitTimer = 0f; // Temporizador para controlar cuánto tiempo el enemigo espera antes de generar una nueva posición objetivo en estado Idle
    private bool isWaiting = false; // Variable para controlar si el enemigo está esperando antes de generar una nueva posición objetivo en estado Idle
    [HideInInspector] public bool isDefeated = false; // Variable para controlar si el enemigo está derrotado

    private float wakeUpTimer; // Temporizador para que el enemigo empiece a actuar tras un poco de tiempo tras ser activado

    void OnEnable()
    {
        wakeUpTimer = 1f;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (attack != null)
        {
            if (attack.TryGetComponent(out Animator atkAnim))
            {
                attackAnim = atkAnim;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = EnemyState.Idle;

        enemyProjectiles = new List<GameObject>();
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

        if(wakeUpTimer > 0) { 
            wakeUpTimer -= Time.deltaTime; 
            return;
        }

        if (player != null && currentState != EnemyState.Attack)
        {
            // Si el jugador está dentro del radio de alerta, el enemigo se mantiene alerta
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            //Si esta en rango de ataque, ataca
            if(distanceToPlayer <= attackRange)
            {
                currentState = EnemyState.Attack;
            } else if(distanceToPlayer <= detectionRadius || isAlerted)
            {
                currentState = EnemyState.Chasing;
            } else
            {
                currentState = EnemyState.Idle;
            }
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    //Metodo que define el estado idle
    private void Idle()
    {
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
    }

    //Metodo que define el estado chase
    private void Chase()
    {
        if(player == null) return;
        // El enemigo se mueve hacia el jugador
        isWaiting = false;
        direction = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + (Vector3) direction * enemyType.GetMoveSpeed * Time.fixedDeltaTime);
    }

    // Metodo que define el estado attack, definido por herencia
    protected virtual void Attack()
    {
        currentState = EnemyState.Chasing;
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

        if (distance <= detectionRadius)
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

            // Al chocar, impedir que sea empujado
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();
            player.TakeDamage(enemyType.GetBodyDamage);

            // Al chocar, impedir que sea empujado
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Al dejar de chocar que se vuelva a mover de forma normal
            rb.bodyType = RigidbodyType2D.Dynamic;
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

    public void SetDefeated(bool defeated = true)
    {
        isDefeated = defeated;
        rb.linearVelocity = Vector2.zero;
        hitbox.enabled = !defeated;
        if (defeated)
        {
            StopAttack();
            anim.SetTrigger("hurt");
        }
    }
    public void ActivateAttack()
    {
        if(attack == null) return;
        attack.SetActive(true);
        if(attackAnim != null) attackAnim.SetTrigger("attack");
    }

    public void StopAttack()
    {
        if(attack == null) return;
        attack.SetActive(false);
    }

    public void DestroyEnemyProjectiles()
    {
        foreach(GameObject projectile in enemyProjectiles)
        {
            if (projectile != null)
            {
                Destroy(projectile);
            }
        }
        enemyProjectiles.Clear();
    }
}
