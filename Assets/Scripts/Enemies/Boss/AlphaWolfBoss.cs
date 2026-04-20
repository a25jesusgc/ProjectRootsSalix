using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaWolfBoss : MonoBehaviour
{
    private Transform player;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;
    private AudioSource audioSource;

    [SerializeField] private Enemy enemy;
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private Transform dashIndicator;
    [SerializeField] private Transform leftDashPoint;
    [SerializeField] private Transform rightDashPoint;
    [SerializeField] private Transform arenaCenter;
    [SerializeField] private float minArenaY;
    [SerializeField] private float maxArenaY;
    [SerializeField] private Animator clawAttack;
    [SerializeField] private EnemyAttack dashAttack;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip[] sfx;

    private const float CLAW_ATTACK_RANGE = 3f;
    private const float DASH_INDICATOR_TIME = 0.25f;
    private const float DASH_SPEED = 80f;
    private const float MULTIDASH_INDICATOR_TIME = 0.15f;
    private const float MULTIDASH_SPEED = 160f;
    private const float BULLET_SPEED = 18f;
    private const float BULLET_SPREAD = 5f;
    private const float BULLET_FIRE_RATE = 0.2f;
    private const float BARRAGE_FIRE_RATE = 0.05f;


    private Vector2 mov;
    private Vector2 attackDirection;
    private bool chooseAttack;
    private int chosenAttack;
    private int lastAttackUsed;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();

        dashAttack.SetMultiplier(1f);

        lastAttackUsed = -1;
    }

    void Update()
    {
        if(player == null || GlobalUtils.pause) return;

        mov = (player.position - transform.position).normalized;

        if (chooseAttack)
        {
            do{
                chosenAttack = Random.Range(0, enemyHealth.GetHealthPercentage <= 0.5f ? 5 : 3);
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

    private IEnumerator GetAttack(int attackIndex)
    {
        switch (attackIndex)
        {
            case 0:
                return ClawAttack();
            case 1:
                return DashAttack();
            case 2:
                return ShootAttack();
            case 3:
                return BarrageAttack();
            case 4:
                return MultiDashAttack();
            default:
                return ClawAttack();
        }
    }

    // Se acerca al jugador y le da un arañazo
    private IEnumerator ClawAttack()
    {
        anim.SetBool("walking", true);
        float distance = Vector3.Distance(player.position, transform.position);
        while (distance > CLAW_ATTACK_RANGE)
        {
            distance = Vector3.Distance(player.position, transform.position);
            mov = (player.position - transform.position).normalized;
            anim.SetFloat("x", mov.x);
            anim.SetFloat("y", mov.y);

            rb.linearVelocity = mov * enemy.GetMoveSpeed;

            yield return null;
        }

        // Claw Attack
        anim.SetBool("walking", false);
        anim.SetTrigger("attack");
        PlaySfxAtPoint(0);
        rb.linearVelocity = Vector2.zero;

        attackDirection = mov;
        clawAttack.transform.localPosition = attackDirection * CLAW_ATTACK_RANGE;
        clawAttack.transform.GetChild(0).rotation = Quaternion.LookRotation(Vector3.forward, attackDirection);

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Se abalanza de un lado a otro del campo
    private IEnumerator DashAttack()
    {
        anim.SetBool("dashing", true);
        anim.SetTrigger("jump");
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de salto

        col.enabled = false;
        dashAttack.gameObject.SetActive(true);
        int dashAmount = Random.Range(2, 5);
        for(int i = 0; i < dashAmount; i++)
        {
            yield return StartCoroutine(DashCoroutine());
        }

        yield return new WaitForSeconds(1f);

        transform.position = new Vector3(Random.Range(leftDashPoint.position.x + 5, rightDashPoint.position.x - 5), Random.Range(minArenaY, maxArenaY), 0);
        col.enabled = true;
        dashAttack.gameObject.SetActive(false);
        anim.SetBool("dashing", false);
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de regreso

        anim.SetFloat("x", mov.x);
        anim.SetFloat("y", mov.y);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Dispara con el cañón hacia el jugador
    private IEnumerator ShootAttack()
    {
        anim.SetBool("shooting", true);
        anim.SetTrigger("open_cannon");
        PlaySfx(4);
        yield return new WaitForSeconds(0.65f); // Espera a terminar la animación de abrir cañón

        yield return new WaitForSeconds(0.25f); // Pequeña espera para dar tiempo de reacción

        int bulletAmount = Random.Range(10, 24);
        for (int i = 0; i < bulletAmount; i++)
        {
            anim.SetFloat("x", mov.x);
            anim.SetFloat("y", mov.y);

            //Instancia la bala donde se encuentra el enemigo
            Vector2 direction = Quaternion.Euler(0f, 0f, Random.Range(-BULLET_SPREAD, BULLET_SPREAD)) * mov;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
            //Le da velocidad
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * BULLET_SPEED;
            // Le pasa el daño que debe hacer a bullet
            bullet.GetComponent<EnemyProjectile>().damage = Mathf.RoundToInt(enemy.GetAttackDamage * 0.35f);
            PlaySfx(5);

            yield return new WaitForSeconds(BULLET_FIRE_RATE); // Cadencia de disparo
        }
        anim.SetBool("shooting", false);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Dispara con el cañón en todas direcciones
    private IEnumerator BarrageAttack()
    {
        // Va al centro de la arena primero
        anim.SetBool("walking", true);
        float distance = Vector3.Distance(arenaCenter.position, transform.position);
        Vector2 arenaDirection;
        while (distance > 0.2f)
        {
            distance = Vector3.Distance(arenaCenter.position, transform.position);
            arenaDirection = (arenaCenter.position - transform.position).normalized;
            anim.SetFloat("x", arenaDirection.x);
            anim.SetFloat("y", arenaDirection.y);

            rb.linearVelocity = arenaDirection * enemy.GetMoveSpeed;

            yield return null;
        }
        anim.SetBool("walking", false);
        rb.linearVelocity = Vector2.zero;

        // Realiza la ráfaga de disparos

        anim.SetBool("shooting", true);
        anim.SetTrigger("open_cannon");
        PlaySfx(4);
        yield return new WaitForSeconds(0.65f); // Espera a terminar la animación de abrir cañón

        yield return new WaitForSeconds(0.25f); // Pequeña espera para dar tiempo de reacción

        int bulletAmount = Random.Range(80, 100);
        Vector2 aim = mov;
        for (int i = 0; i < bulletAmount; i++)
        {
            anim.SetFloat("x", aim.x);
            anim.SetFloat("y", aim.y);

            //Instancia la bala donde se encuentra el enemigo
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.LookRotation(Vector3.forward, aim));
            //Le da velocidad
            bullet.GetComponent<Rigidbody2D>().linearVelocity = aim * BULLET_SPEED;
            // Le pasa el daño que debe hacer a bullet
            bullet.GetComponent<EnemyProjectile>().damage = Mathf.RoundToInt(enemy.GetAttackDamage * 0.35f);

            aim = Quaternion.Euler(0f, 0f, 16f) * aim;
            PlaySfx(5);

            yield return new WaitForSeconds(BARRAGE_FIRE_RATE); // Cadencia de disparo
        }
        anim.SetBool("shooting", false);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Se abalanza de un lado a otro del campo varias veces
    private IEnumerator MultiDashAttack()
    {
        anim.SetBool("dashing", true);
        anim.SetTrigger("jump");
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de salto

        col.enabled = false;
        dashAttack.gameObject.SetActive(true);
        int dashAmount = Random.Range(1, 4);
        for(int i = 0; i < dashAmount; i++)
        {
            yield return StartCoroutine(MultiDashCoroutine());
        }

        yield return new WaitForSeconds(1f);

        transform.position = new Vector3(Random.Range(leftDashPoint.position.x + 5, rightDashPoint.position.x - 5), Random.Range(minArenaY, maxArenaY), 0);
        col.enabled = true;
        dashAttack.gameObject.SetActive(false);
        anim.SetBool("dashing", false);
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de regreso
        
        anim.SetFloat("x", mov.x);
        anim.SetFloat("y", mov.y);

        yield return StartCoroutine(EndAttackRecovery());
    }


    private IEnumerator DashCoroutine()
    {
        bool leftSide = Random.Range(0, 2) == 0;
        
        anim.SetFloat("x", leftSide ? 1 : -1);
        anim.SetFloat("y", 0);

        float yPos = Random.Range(player.transform.position.y - 2f, player.transform.position.y + 2f);
        dashIndicator.position = new Vector3(dashIndicator.position.x, Mathf.Clamp(yPos, minArenaY, maxArenaY), 0f);
        transform.position = new Vector3(leftSide ? leftDashPoint.position.x : rightDashPoint.position.x, dashIndicator.position.y, 0);

        dashIndicator.gameObject.SetActive(true);
        PlaySfxAtPoint(2);
        yield return new WaitForSeconds(DASH_INDICATOR_TIME);
        dashIndicator.gameObject.SetActive(false);

        rb.linearVelocity = new Vector2(leftSide ? DASH_SPEED : -DASH_SPEED, 0);
        PlaySfx(3);

        yield return new WaitUntil(() => leftSide ? transform.position.x > rightDashPoint.position.x : transform.position.x < leftDashPoint.position.x);

        rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator MultiDashCoroutine()
    {
        float[] dashPositions = new float[3];

        transform.position = new Vector3(rightDashPoint.position.x, dashIndicator.position.y, 0);

        for(int i = 0; i < 3; i++)
        {

            float yPos = Random.Range(player.transform.position.y - (3f * i), player.transform.position.y + (3f * i));
            dashIndicator.position = new Vector3(dashIndicator.position.x, Mathf.Clamp(yPos, minArenaY, maxArenaY), 0f);

            dashPositions[i] = dashIndicator.position.y;

            dashIndicator.gameObject.SetActive(true);
            PlaySfxAtPoint(2);
            yield return new WaitForSeconds(MULTIDASH_INDICATOR_TIME);
            dashIndicator.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < 3; i++)
        {
            bool leftSide = Random.Range(0, 2) == 0;
            
            anim.SetFloat("x", leftSide ? 1 : -1);
            anim.SetFloat("y", 0);

            transform.position = new Vector3(leftSide ? leftDashPoint.position.x : rightDashPoint.position.x, dashPositions[i], 0);

            rb.linearVelocity = new Vector2(leftSide ? MULTIDASH_SPEED : -MULTIDASH_SPEED, 0);
            PlaySfx(3);

            yield return new WaitUntil(() => leftSide ? transform.position.x > rightDashPoint.position.x : transform.position.x < leftDashPoint.position.x);

            rb.linearVelocity = Vector2.zero;
        }
    }

    private IEnumerator EndAttackRecovery()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.5f));

        chooseAttack = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();
            player.TakeDamage(enemy.GetBodyDamage);
        }
    }

    public void ActivateClawAttack()
    {
        clawAttack.gameObject.SetActive(true);
        clawAttack.SetFloat("x", attackDirection.x);
        clawAttack.SetFloat("y", attackDirection.y);
        PlaySfx(1);
        clawAttack.SetTrigger("attack");
            
    }

    public void StopClawAttack()
    {
        clawAttack.gameObject.SetActive(false);
    }

    private void PlaySfx(int index)
    {
        audioSource.spatialBlend = 1;
        audioSource.clip = sfx[index];
        audioSource.Play();
    }
    private void PlaySfxAtPoint(int index)
    {
        audioSource.spatialBlend = 0;
        audioSource.clip = sfx[index];
        audioSource.Play();
    }
}
