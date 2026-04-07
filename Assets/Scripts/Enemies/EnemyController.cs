using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chasing
    }

    [SerializeField] private Transform Player;

    public float speed = 3f;
    public float detectionRadius = 5f;
    public float alertRadius = 3f;

    private Rigidbody2D rb;
    private EnemyState currentState;

    // Variable para controlar si el enemigo está alerta
    private bool isAlerted;
    // Tiempo que el enemigo permanece alerta después de que el jugador dispara cerca de él
    private float alertTime = 5f;
    // Temporizador para controlar cuánto tiempo el enemigo permanece alerta
    private float alertTimer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void FixedUpdate()
    {
        // Verificar la distancia al jugador
        if (Player == null) return;

        // Si el jugador está dentro del radio de alerta, el enemigo se mantiene alerta
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);
        // Si el jugador está dentro del radio de detección o el enemigo está alerta, cambia al estado de persecución
        if (distanceToPlayer <= detectionRadius || isAlerted)
        {
            currentState = EnemyState.Chasing;
        }
        else
        {
            currentState = EnemyState.Idle;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                // Aquí puedes agregar lógica para el comportamiento de patrulla o quedarse quieto

                break;
            case EnemyState.Chasing:
                // El enemigo se mueve hacia el jugador
                Vector3 direction = (Player.position - transform.position).normalized;
                rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // Si el jugador dispara cerca del enemigo, el enemigo se alerta
            float distance = Vector2.Distance(transform.position, collision.transform.position);

            if (distance <= alertRadius)
            {
                isAlerted = true;
                alertTimer = 0f; // Reiniciar el temporizador de alerta
            }
        }
    }

    // Función para recibir daño
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();
            player.TakeDamage(10);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);


    }
}
