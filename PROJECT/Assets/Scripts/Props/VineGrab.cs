using UnityEngine;

public class VineGrab : MonoBehaviour
{
    // Referencia al transform del gancho
    private Transform vine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Si tiene referencia al gancho, se enlaza a él
        if(vine != null) transform.position = vine.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vine"))
        {
            if (collision.TryGetComponent(out VineProjectile vineProjectile))
            {
                // Si detecta un gancho y el ganchó no agarró nada, agarra este objeto
                if (!vineProjectile.grabbed)
                {
                    // El gancho ya no puede agarrar otras cosas
                    vineProjectile.grabbed = true;
                    // Este objeto seguirá al gancho vine
                    vine = collision.transform;
                    // El gancho se agarra a este objeto
                    vineProjectile.AttachTarget(collision.transform);
                    // Y el gancho se desagarra para volver al jugador mientras arrastra este objeto
                    vineProjectile.RemoveTarget();
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Si el objeto choca con el jugador, deja de seguir al gancho y se queda quieto
            vine = null;
        }
    }
}
