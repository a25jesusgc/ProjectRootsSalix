using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController : MonoBehaviour
{
    public Transform player {get; private set;}

    public Animator anim {get; private set;}
    public Rigidbody2D rb {get; private set;}
    public Collider2D col {get; private set;}
    public AudioSource audioSource {get; private set;}

    public Enemy enemy;
    public EnemyHealth enemyHealth;
    public float secondPhaseHealth = 0.5f;
    public Transform arenaCenter;
    public Transform arenaMin;
    public Transform arenaMax;
    public AudioClip[] sfx;

    [HideInInspector] public Vector2 mov;
    [HideInInspector] public Vector2 attackDirection;
    [HideInInspector] public bool chooseAttack;
    [HideInInspector] public int chosenAttack;
    [HideInInspector] public int lastAttackUsed;
    [HideInInspector] public bool isDefeated;

    public int firstPhaseAttacksCount;
    public int secondPhaseAttacksCount;

    public virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();

        lastAttackUsed = -1;
    }
    public virtual void Update()
    {
        if(player == null || GlobalUtils.pause || isDefeated) return;

        if (!isDefeated && enemyHealth.GetHealthPercentage <= 0f)
        {
            isDefeated = true;
            StopAllCoroutines();
            rb.linearVelocity = Vector2.zero;
            return;
        }

        mov = (player.position - transform.position).normalized;

        if (chooseAttack)
        {
            do{
                chosenAttack = Random.Range(0, enemyHealth.GetHealthPercentage <= secondPhaseHealth ? secondPhaseAttacksCount : firstPhaseAttacksCount);
            }while(chosenAttack == lastAttackUsed);
            
            chooseAttack = false;
            lastAttackUsed = chosenAttack;
            
            StartCoroutine(GetAttack(chosenAttack));
        }

    }

    public void SetPlayer(Transform target)
    {
        player = target;
        chooseAttack = true;
    }

    public abstract IEnumerator GetAttack(int attackIndex);

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(isDefeated) return;
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();
            player.TakeDamage(enemy.GetBodyDamage);
        }
    }

    public void PlaySfx(int index)
    {
        audioSource.spatialBlend = 1;
        audioSource.clip = sfx[index];
        audioSource.Play();
    }
    public void PlaySfxAtPoint(int index)
    {
        audioSource.spatialBlend = 0;
        audioSource.clip = sfx[index];
        audioSource.Play();
    }

    public float GetDayCycleAttackMultiplier()
    {
        float multiplier = 1f;

        if (DayCycleManager.instance.IsDay)
        {
            return enemy.GetDayAttackMultipler;
        }
        else if (DayCycleManager.instance.IsNight)
        {
            return enemy.GetNightAttackMultipler;
        }

        return multiplier;
    }
}
