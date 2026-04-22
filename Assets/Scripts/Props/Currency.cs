using UnityEngine;

public class Currency : MonoBehaviour
{
    [SerializeField] private int amount;
    private Rigidbody2D rb;

    private Vector2 startSpeed;
    private float startRotation;
    private float duration = 0.5f;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        startSpeed = randomDirection * Random.Range(6f, 12f);
        startRotation = Random.Range(-180f, 180f);
    }

    void Update()
    {
        if (timer < duration)
        {
            timer += Time.deltaTime;
            rb.linearVelocity = Vector2.Lerp(startSpeed, Vector2.zero, timer / duration);
            rb.angularVelocity = Mathf.Lerp(startRotation, 0, timer / duration);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerData.GetInstance.ChangeCurrency(amount);
            PlayerCurrency.instance.ChangeCurrency(amount);
            Destroy(gameObject);
        }
    }
}
