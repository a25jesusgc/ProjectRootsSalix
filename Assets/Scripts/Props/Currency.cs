using UnityEngine;

// Prop de moneda que al recogerla se añade dinero
public class Currency : MonoBehaviour
{
    // Cantidad de dinero que vale este prop
    [SerializeField] private int amount;
    private Rigidbody2D rb;

    // Valores para gestionar su desplazamiento al ser instanciado
    private Vector2 startSpeed;
    private float startRotation;
    private float duration = 0.65f;
    private float timer;


    // Al spawnear, se mueve en una dirección aleatoria, con una velocidad y rotación aleatoria
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        startSpeed = randomDirection * Random.Range(6f, 12f);
        startRotation = Random.Range(-180f, 180f);
    }


    // Durante el tiempo asignado en duration, va frenando su movimiento y rotación hasta detenerse
    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            rb.linearVelocity = Vector2.Lerp(startSpeed, Vector2.zero, timer / duration);
            rb.angularVelocity = Mathf.Lerp(startRotation, 0, timer / duration);
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
