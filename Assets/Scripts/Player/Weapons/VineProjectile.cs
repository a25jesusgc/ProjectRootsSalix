using System.Collections.Generic;
using UnityEngine;

public class VineProjectile : MonoBehaviour
{
    public PlayerVine playerVine;
    private Transform target;
    private Rigidbody2D rb;
    private LineTargets lineTargets;
    [SerializeField] private AudioSource vineSound;

    private float speed;
    private Vector2 direction;

    private bool attached;

    private const float MAX_TRAVEL_TIME = 0.25f;
    private const float MAX_RANGE = 8f;
    private const float PLAYER_VINE_SPEED = 3f;
    private float travelTime;

    public bool grabbed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lineTargets = GetComponent<LineTargets>();
    }

    void Update()
    {
        if(!attached) travelTime += Time.deltaTime;
        if(travelTime >= MAX_TRAVEL_TIME)
        {
            playerVine.StopVine();
        }
        if (target != null)
        {
            if(Vector3.Distance(target.position, playerVine.transform.position) > MAX_RANGE)
            {
                playerVine.StopVine();
            }
        }
    }

    void FixedUpdate()
    {
        if (attached)
        {
            if (target != null)
            {
                transform.position = target.position;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                direction = playerVine.transform.position - transform.position;
                rb.linearVelocity = direction.normalized * speed;
                if (Vector3.Distance(playerVine.transform.position, transform.position) < 0.5)
                {
                    playerVine.playerController.ResetSpeed();
                    Destroy(gameObject);
                }
            }
        }
    }

    public void SetPlayerVine(PlayerVine value)
    {
        playerVine = value;
        lineTargets.SetTargets(new List<Transform>() {playerVine.transform, transform});
        playerVine.playerController.SetSpeed(PLAYER_VINE_SPEED);
    }
    public void SetVelocity(Vector2 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        rb.linearVelocity = direction.normalized * speed;
    }
    public void AttachTarget(Transform newTarget)
    {
        target = newTarget;
        attached = true;
        vineSound.Play();
    }
    public void RemoveTarget()
    {
        target = null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !attached)
        {
            if (collision.TryGetComponent(out EnemyHealth enemy))
            {
                playerVine.SetTarget(enemy);
                AttachTarget(collision.transform);
            }
        }
        if (collision.CompareTag("Player"))
        {
            if (attached)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ReturnVine()
    {
        attached = true;
        target = null;
    }
}
