using UnityEngine;

public class HealingPond : MonoBehaviour
{
    [SerializeField] private float percentage;
    [SerializeField] private int maxUsages;

    private int usages = 0;
    private Animator anim;
    private AudioSource audioSource;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(usages >= maxUsages) return;

        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerHealthController playerHealth))
            {
                if(playerHealth.GetHealthPercentage == 1f) return;
                playerHealth.Heal(Mathf.RoundToInt(percentage * playerHealth.GetMaxHP));
                usages++;
                anim.SetFloat("usages", usages);
                audioSource.Play();
            }
        }
    }
}
