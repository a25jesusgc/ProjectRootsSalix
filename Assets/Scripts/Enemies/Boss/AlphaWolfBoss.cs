using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaWolfBoss : MonoBehaviour
{
    private Transform player;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private Enemy enemy;
    [SerializeField] private Transform dashIndicator;
    [SerializeField] private Transform leftDashPoint;
    [SerializeField] private Transform rightDashPoint;
    [SerializeField] private float minArenaY;
    [SerializeField] private float maxArenaY;
    [SerializeField] private EnemyAttack dashAttack;

    private const float DASH_INDICATOR_TIME = 0.25f;
    private const float DASH_SPEED = 80f;
    private const float MULTIDASH_INDICATOR_TIME = 0.15f;
    private const float MULTIDASH_SPEED = 160f;


    private bool chooseAttack;
    private int chosenAttack;
    private int lastAttackUsed;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        dashAttack.SetMultiplier(1f);

        lastAttackUsed = -1;
    }

    void Update()
    {
        if(player == null || GlobalUtils.pause) return;

        if (chooseAttack)
        {
            do{
                chosenAttack = Random.Range(0, 5);
            }while(chosenAttack == lastAttackUsed);

            Debug.Log("ATTACK " + chosenAttack);
            
            chooseAttack = false;
            
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
        yield return new WaitForSeconds(1f);
        chooseAttack = true;
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

        transform.position = new Vector3(Random.Range(leftDashPoint.position.x, rightDashPoint.position.x), Random.Range(minArenaY, maxArenaY), 0);
        col.enabled = true;
        dashAttack.gameObject.SetActive(false);
        anim.SetBool("dashing", false);
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de regreso

        chooseAttack = true;
    }

    // Dispara con el cañón hacia el jugador
    private IEnumerator ShootAttack()
    {
        yield return new WaitForSeconds(1f);
        chooseAttack = true;
    }

    // Dispara con el cañón en todas direcciones
    private IEnumerator BarrageAttack()
    {
        yield return new WaitForSeconds(1f);
        chooseAttack = true;
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

        transform.position = new Vector3(Random.Range(leftDashPoint.position.x, rightDashPoint.position.x), Random.Range(minArenaY, maxArenaY), 0);
        col.enabled = true;
        dashAttack.gameObject.SetActive(false);
        anim.SetBool("dashing", false);
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de regreso

        chooseAttack = true;
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
        yield return new WaitForSeconds(DASH_INDICATOR_TIME);
        dashIndicator.gameObject.SetActive(false);

        rb.linearVelocity = new Vector2(leftSide ? DASH_SPEED : -DASH_SPEED, 0);

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

            yield return new WaitUntil(() => leftSide ? transform.position.x > rightDashPoint.position.x : transform.position.x < leftDashPoint.position.x);

            rb.linearVelocity = Vector2.zero;
        }
    }
}
