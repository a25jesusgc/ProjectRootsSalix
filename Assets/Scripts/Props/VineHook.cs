using UnityEngine;

public class VineHook : MonoBehaviour
{
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
                vine = vineProjectile;
                if (!vine.grabbed)
                {
                    vine.grabbed = true;
                    vine.AttachTarget(transform);
                    player = vineProjectile.playerVine.playerController;
                    player.StartHookJump(transform);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(player != null)
            {
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
                TargetReached();
            }
        }
    }

    private void TargetReached()
    {
        player.StopHookJump();
        player = null;
        vine.playerVine.StopVine();
    }
}
