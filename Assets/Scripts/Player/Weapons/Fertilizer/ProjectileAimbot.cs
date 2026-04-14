using UnityEngine;

public class ProjectileAimbot : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private Transform target;

    private float speed = 20f;
    private float rotationSpeed = 80f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && target == null)
        {
            target = collision.transform;
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            if (Mathf.Abs(Vector2.Angle(direction, rb.linearVelocity.normalized)) < 120)
            {
                rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, direction * speed, Time.deltaTime * rotationSpeed);
                rb.gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.linearVelocity.normalized);
            }
        }
    }
}
