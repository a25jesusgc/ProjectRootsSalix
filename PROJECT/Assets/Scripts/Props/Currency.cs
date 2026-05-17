using UnityEngine;

// Prop de moneda que al recogerla se añade dinero
public class Currency : MonoBehaviour
{
    // Cantidad de dinero que vale este prop
    [SerializeField] private int amount;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Valores para gestionar su desplazamiento al ser instanciado
    private Vector2 startSpeed;
    private float startRotation;
    private const float MOVE_DURATION = 0.65f;
    private float timer;
    private const float LIFETIME = 60f;
    private const float FADETIME = 50f;
    private float lifeTimer;


    // Al spawnear, se mueve en una dirección aleatoria, con una velocidad y rotación aleatoria
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        startSpeed = randomDirection * Random.Range(6f, 12f);
        startRotation = Random.Range(-180f, 180f);
    }


    // Durante el tiempo asignado en duration, va frenando su movimiento y rotación hasta detenerse
    void Update()
    {
        if (timer < MOVE_DURATION)
        {
            timer += Time.deltaTime;
            rb.linearVelocity = Vector2.Lerp(startSpeed, Vector2.zero, timer / MOVE_DURATION);
            rb.angularVelocity = Mathf.Lerp(startRotation, 0, timer / MOVE_DURATION);
        }

        if (lifeTimer < LIFETIME)
        {
            lifeTimer += Time.deltaTime;
            if(lifeTimer >= FADETIME) sr.color = new Color(1f, 1f, 1f, 0.75f);
        }
        else
        {
            Destroy(gameObject);
        }

    }


    // Al entrar en contacto con el jugador, se destruye el prop y se añade el dinero
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Añade la cantidad de dinero al valor de dinero del jugador
            PlayerData.GetInstance.ChangeCurrency(amount);

            // Muestra en la UI de dinero la alteración económica
            PlayerCurrency.instance.ChangeCurrency(amount);

            // Destruye la moneda al ser adquirida
            Destroy(gameObject);
        }
    }
}
