using System.Collections;
using UnityEngine;

public class BearLumberjackBoss : BossController
{
    [SerializeField] private Animator biteAttack;
    [SerializeField] private GameObject slamAttack;
    [SerializeField] private Transform shadow;
    private const float BITE_ATTACK_RANGE = 2.5f;
    private const float BITE_ATTACK_OFFSET = 1f;
    private const float SLAM_TIME = 2f;
    private const float SLAM_HEIGHT = 5f;
    private const float SLAM_HEIGHT_OFFSET = 1.5f;
    private const float SLAM_FALL_SPEED = 40f;
    private const float SHADOW_OFFSET = -2.25f;
    private const float WHIRLWIND_SPEED = 20f;
    private const float WHIRLWIND_ACCELERATION = 10f;
    private const float WHIRLWIND_ANGULAR_SPEED = 0.5f;
    private const float WHIRLWIND_DURATION = 8f;

    private Vector2 whirlDirection;
    private bool isWhirling;
    private float whirlSpeed;

    public override IEnumerator GetAttack(int attackIndex)
    {
        switch (attackIndex)
        {
            /*case 0:
                return BiteAttack();
            case 1:
                return SlamAttack();
            case 2:
                return WhirlwindAttack();
            case 3:
                return TreeThrowAttack();
            case 4:
                return TreeRainAttack();*/
            default:
                return WhirlwindAttack();
        }
    }

    // Se acerca al jugador y le da un mordisco
    private IEnumerator BiteAttack()
    {
        anim.SetBool("walking", true);
        float distance = Vector3.Distance(player.position, transform.position);
        while (distance > BITE_ATTACK_RANGE)
        {
            distance = Vector3.Distance(player.position, transform.position);
            mov = (player.position - transform.position).normalized;
            anim.SetFloat("x", mov.x);
            anim.SetFloat("y", mov.y);

            rb.linearVelocity = mov * enemy.GetMoveSpeed;

            yield return null;
        }

        // Bite Attack
        anim.SetBool("walking", false);
        anim.SetTrigger("attack");
        //PlaySfxAtPoint(0);
        rb.linearVelocity = Vector2.zero;

        attackDirection = mov;
        biteAttack.transform.localPosition = attackDirection * BITE_ATTACK_OFFSET;
        biteAttack.transform.rotation = Quaternion.LookRotation(Vector3.forward, attackDirection);

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Da un salto y aparece sobre el jugador, para luego caer dando un golpe en el suelo
    private IEnumerator SlamAttack()
    {
        anim.SetTrigger("jump");
        anim.SetBool("on_ground", false);
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de salto

        // Ha saltado y persigue desde el aire al jugador
        col.enabled = false;

        float time = SLAM_TIME;
        while (time > 0f)
        {
            time -= Time.deltaTime;

            transform.position = player.position + Vector3.up * SLAM_HEIGHT_OFFSET;

            yield return null;
        }

        // Selecciona punto objetivo del slam
        Vector3 targetPosition = transform.position;

        // Se posiciona en el punto elevado para caer
        float airValue = SLAM_HEIGHT;
        transform.position = targetPosition + Vector3.up * airValue;
        shadow.localPosition = Vector3.up * (-airValue + SHADOW_OFFSET);

        // Aparece
        anim.SetTrigger("slam");
        yield return new WaitForSeconds(0.25f);

        // Cae e impacta el suelo
        while (airValue > 0f)
        {
            airValue -= Time.deltaTime * SLAM_FALL_SPEED;
            if(airValue < 0) airValue = 0;

            transform.position = targetPosition + Vector3.up * airValue;
            shadow.localPosition = Vector3.up * (-airValue + SHADOW_OFFSET);

            yield return null;
        }
        anim.SetBool("on_ground", true);
        
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Comienza a girar sobre sí mismo, rebotando contra las paredes
    private IEnumerator WhirlwindAttack()
    {
        // Comienza a girar
        anim.SetTrigger("whirlwind");
        yield return new WaitForSeconds(0.2f);
        
        isWhirling = true;
        whirlDirection = mov;
        rb.linearVelocity = Vector2.zero;

        float duration = WHIRLWIND_DURATION;
        whirlSpeed = 5f;
        while(duration > 0f)
        {
            duration -= Time.deltaTime;

            whirlSpeed = Mathf.MoveTowards(whirlSpeed, WHIRLWIND_SPEED, Time.deltaTime * WHIRLWIND_ACCELERATION);
            if(Vector2.Angle(whirlDirection, mov) < 120f) whirlDirection = Vector2.MoveTowards(whirlDirection, mov, Time.deltaTime * WHIRLWIND_ANGULAR_SPEED).normalized;
            //rb.linearVelocity = whirlDirection * whirlSpeed;

            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, arenaMin.position.x, arenaMax.position.x), Mathf.Clamp(transform.position.y, arenaMin.position.y, arenaMax.position.y), transform.position.z);

            yield return null;
        }

        isWhirling = false;

        // Se cansa
        anim.SetBool("is_tired", true);
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(1f);

        anim.SetBool("is_tired", false);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Se acerca a un árbol y lo tala para lanzárselo al jugador
    private IEnumerator TreeThrowAttack()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Salta al fondo de la arena, donde comienza a cortar árboles y lanzarlos hacia abajo de la arena
    private IEnumerator TreeRainAttack()
    {
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(EndAttackRecovery());
    }


    private IEnumerator EndAttackRecovery()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.5f));

        chooseAttack = true;
    }
    public void ActivateBiteAttack()
    {
        biteAttack.gameObject.SetActive(true);
        //PlaySfx(1);
        biteAttack.SetTrigger("attack");
    }

    public void StopBiteAttack()
    {
        biteAttack.gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Bullet"))
        {
            whirlDirection = Vector2.Reflect(whirlDirection, collision.GetContact(0).normal);
        }
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();
            player.TakeDamage(isWhirling ? enemy.GetAttackDamage : enemy.GetBodyDamage);
        }
    }

    void FixedUpdate()
    {
        if(isWhirling) rb.linearVelocity = whirlDirection * whirlSpeed;
    }
}
