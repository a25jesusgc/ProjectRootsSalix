using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearLumberjackBoss : BossController
{
    [SerializeField] private Animator biteAttack;
    [SerializeField] private Transform shadow;
    [SerializeField] private List<Transform> trees;
    [SerializeField] private AudioSource whirlSFX;
    [SerializeField] private GameObject treeAttack;
    private const float BITE_ATTACK_RANGE = 3f;
    private const float BITE_ATTACK_OFFSET = 1.25f;
    private const float JUMPING_SPEED = 12f;
    private const float SLAM_TIME = 2f;
    private const float SLAM_HEIGHT = 4f;
    private const float SLAM_HEIGHT_OFFSET = 1.5f;
    private const float SLAM_FALL_SPEED = 40f;
    private const float SHADOW_OFFSET = -2.25f;
    private const float WHIRLWIND_SPEED = 20f;
    private const float WHIRLWIND_ACCELERATION = 10f;
    private const float WHIRLWIND_ANGULAR_SPEED = 0.5f;
    private const float WHIRLWIND_DURATION = 8f;
    private const float THROWN_TREE_SPEED = 16f;

    private Vector2 whirlDirection;
    private Vector2 lastVelocityDir;
    private bool isWhirling;
    private float whirlSpeed;

    public override void Update()
    {
        base.Update();
        lastVelocityDir = rb.linearVelocity.normalized;
    }

    public override IEnumerator GetAttack(int attackIndex)
    {
        switch (attackIndex)
        {
            case 0:
                return BiteAttack();
            case 1:
                return SlamAttack();
            case 2:
                return TreeThrowAttack();
            case 3:
                return WhirlwindAttack();
            case 4:
                return TreeRainAttack();
            default:
                return BiteAttack();
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
        PlaySfx(6);
        anim.SetTrigger("attack");
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
        col.enabled = false;
        anim.SetBool("on_ground", false);
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de salto

        // Ha saltado y persigue desde el aire al jugador

        float time = SLAM_TIME;
        while (time > 0f)
        {
            time -= Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, player.position + Vector3.up * SLAM_HEIGHT_OFFSET, Time.deltaTime * JUMPING_SPEED);

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
        PlaySfx(2);
        CameraController.instance.ShakeCamera(1f, 0.25f);
        
        yield return new WaitForSeconds(0.25f);
        col.enabled = true;

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Comienza a girar sobre sí mismo, rebotando contra las paredes
    private IEnumerator WhirlwindAttack()
    {
        // Comienza a girar
        anim.SetTrigger("whirlwind");
        yield return new WaitForSeconds(0.2f);
        
        isWhirling = true;
        whirlSFX.Play();
        whirlDirection = mov;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;

        float duration = WHIRLWIND_DURATION;
        whirlSpeed = 5f;
        while(duration > 0f)
        {
            duration -= Time.deltaTime;

            whirlSpeed = Mathf.MoveTowards(whirlSpeed, WHIRLWIND_SPEED, Time.deltaTime * WHIRLWIND_ACCELERATION);
            if(Vector2.Angle(whirlDirection, mov) < 120f) whirlDirection = Vector2.MoveTowards(whirlDirection, mov, Time.deltaTime * WHIRLWIND_ANGULAR_SPEED).normalized;

            yield return null;
        }

        isWhirling = false;
        whirlSFX.Stop();

        // Se cansa
        anim.SetBool("is_tired", true);
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(Random.Range(2f, 5f));

        anim.SetBool("is_tired", false);

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Se acerca a un árbol y lo tala para lanzárselo al jugador
    private IEnumerator TreeThrowAttack()
    {
        // Elegir el árbol más cercano
        float distance = 15f;
        Transform chosenTree = null;
        foreach (Transform t in trees)
        {
            float treeDistance = Vector3.Distance(transform.position, t.position);
            if(treeDistance < distance)
            {
                chosenTree = t;
                distance = treeDistance;
            }
        }

        Vector3 targetPos = chosenTree.position + Vector3.up * 2f;

        // Salta al árbol
        anim.SetTrigger("jump");
        col.enabled = false;
        anim.SetBool("on_ground", false);
        yield return new WaitForSeconds(1f); // Espera a terminar la animación de salto

        // Va hasta el árbol y aparece donde el árbol para cortarlo
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * JUMPING_SPEED);
            yield return null;
        }
        
        anim.SetTrigger("cut");
        yield return new WaitForSeconds(0.5f); // Espera a terminar la animación de corte
        anim.SetBool("on_ground", true);

        // El árbol cae hacia el jugador
        GameObject thrownTree = Instantiate(treeAttack, chosenTree.position, Quaternion.identity);
        Vector3 treeDirection = (player.transform.position - thrownTree.transform.position).normalized;
        thrownTree.transform.rotation = Quaternion.LookRotation(Vector3.forward, treeDirection);
        thrownTree.GetComponent<Rigidbody2D>().linearVelocity = treeDirection * THROWN_TREE_SPEED;

        yield return new WaitForSeconds(0.5f);

        // Vuelve a la arena
        yield return StartCoroutine(WalkTowardsArena());

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Salta al fondo de la arena, donde comienza a cortar árboles y lanzarlos hacia abajo de la arena
    private IEnumerator TreeRainAttack()
    {
        Transform chosenTree = null;
        for(int i = 0; i < 6; i++)
        {
            Transform newTree = trees[Random.Range(0, trees.Count)];
            while (newTree == chosenTree)
            {
                newTree = trees[Random.Range(0, trees.Count)];
            }
            chosenTree = newTree;
            Vector3 targetPos = chosenTree.position + Vector3.up * 2f;

            // Salta al árbol
            anim.SetTrigger("jump");
            col.enabled = false;
            anim.SetBool("on_ground", false);
            yield return new WaitForSeconds(i == 0 ? 0.55f : 0.25f); // Espera a terminar la animación de salto

            // Aparece donde el árbol para cortarlo
            transform.position = targetPos;
            
            anim.SetTrigger("cut");
            yield return new WaitForSeconds(0.5f); // Espera a terminar la animación de corte
            anim.SetBool("on_ground", true);

            // El árbol cae hacia el jugador
            GameObject thrownTree = Instantiate(treeAttack, chosenTree.position, Quaternion.identity);
            Vector3 treeDirection = chosenTree.up.normalized;
            thrownTree.transform.rotation = Quaternion.LookRotation(Vector3.forward, treeDirection);
            thrownTree.GetComponent<Rigidbody2D>().linearVelocity = treeDirection * THROWN_TREE_SPEED;
        }

        yield return new WaitForSeconds(0.5f);

        // Vuelve a la arena
        yield return StartCoroutine(WalkTowardsArena());

        yield return StartCoroutine(EndAttackRecovery());
    }

    // Camina de vuelta a la arena
    private IEnumerator WalkTowardsArena()
    {
        // Vuelve a la arena
        col.enabled = true;
        anim.SetBool("walking", true);
        float walkTime = 0;
        while (walkTime < 0.5f)
        {
            walkTime += Time.deltaTime;
            mov = (arenaCenter.position - transform.position).normalized;
            anim.SetFloat("x", mov.x);
            anim.SetFloat("y", mov.y);

            rb.linearVelocity = mov * enemy.GetMoveSpeed;

            yield return null;
        }
        anim.SetBool("walking", false);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
    }


    private IEnumerator EndAttackRecovery()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.5f));

        chooseAttack = true;
    }
    public void ActivateBiteAttack()
    {
        biteAttack.gameObject.SetActive(true);
        PlaySfx(0);
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
            Rebound(collision);
        }
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealthController player = collision.gameObject.GetComponent<PlayerHealthController>();
            player.TakeDamage(enemy.GetBodyDamage);
        }
    }

    private void Rebound(Collision2D collision)
    {
        if(!isWhirling) return;
        Vector2 normal = collision.GetContact(0).normal;
        whirlDirection = Vector2.Reflect(lastVelocityDir, normal);
        PlaySfx(5);
    }

    void FixedUpdate()
    {
        if(isWhirling) rb.linearVelocity = whirlDirection * whirlSpeed;
    }
}
