using UnityEngine;

public abstract class BreakableProp : MonoBehaviour, IActivatedProp
{

    [SerializeField] private string bulletTag;
    public Animator anim;
    [SerializeField] private AudioSource breakSFX;
    public bool isBroken;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(isBroken) return;
        if (collision.collider.CompareTag(bulletTag))
        {
            if(anim != null) anim.SetTrigger("break");
            if(breakSFX != null) breakSFX.Play();
            isBroken = true;
            OnBreak();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(isBroken) return;
        if (collision.CompareTag(bulletTag))
        {
            if(anim != null) anim.SetTrigger("break");
            if(breakSFX != null) breakSFX.Play();
            isBroken = true;
            OnBreak();
        }
    }

    public abstract void OnBreak();

    public bool Activated()
    {
        return isBroken;
    }
}
