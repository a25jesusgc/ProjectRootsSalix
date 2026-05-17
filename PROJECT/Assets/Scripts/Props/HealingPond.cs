using UnityEngine;

public class HealingPond : MonoBehaviour
{
    [SerializeField] private float percentage;
    [SerializeField] private int maxUsages;
    [SerializeField] private GameObject healEffect;

    private int usages = 0;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
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
                Instantiate(healEffect, collision.transform.position, Quaternion.identity);
                usages++;
                anim.SetFloat("usages", usages);
            }
        }
    }
}
