using UnityEngine;

public class VineGrab : MonoBehaviour
{
    private Transform vine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(vine != null) transform.position = vine.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vine"))
        {
            if (collision.TryGetComponent(out VineProjectile vineProjectile))
            {
                if (!vineProjectile.grabbed)
                {
                    vineProjectile.grabbed = true;
                    vine = collision.transform;
                    vineProjectile.ReturnVine();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            vine = null;
        }
    }
}
