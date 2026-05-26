using System.Collections;
using UnityEngine;

public class FlammableProp : MonoBehaviour
{
    [SerializeField] private string propID;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource burnSFX;
    [SerializeField] private GameObject burnEffect;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer sr;

    private bool isBurned;

    void Start()
    {
        if(PlayerData.GetInstance.WasEventCompleted(propID)) Destroy(gameObject);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Si ya ardió, da igual
        if(isBurned) return;
        
        // Si una bala impacta contra el prop, comprueba si es daño de fuego, y en caso de serlo, arde
        if (collision.collider.CompareTag("Bullet"))
        {
            if (collision.collider.TryGetComponent(out BulletHit bullet))
            {
                if (bullet.damageType == DamageType.FIRE)
                {
                    StartCoroutine(BurnCoroutine());
                }
            }
        }
    }


    // Corrutina que activa la animación de quemar y, tras un pequeño lapso, desactiva el collider
    private IEnumerator BurnCoroutine()
    {
        isBurned = true;
        if(anim != null) anim.SetTrigger("burn");
        if(burnSFX != null) burnSFX.Play();
        if(burnEffect != null) Instantiate(burnEffect, transform.position, Quaternion.identity);

        PlayerData.GetInstance.CompleteEvent(propID);

        yield return new WaitForSeconds(1f);

        col.enabled = false;
        Destroy(gameObject);
    }
}
