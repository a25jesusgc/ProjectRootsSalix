using UnityEngine;
public class EnemyWolfClaw : EnemyController
{
    //Rango del ataque
    [SerializeField] private float range = 1.5f;
    //Enfriamiento del ataque
    [SerializeField] private float cooldown = 2f; 
    //Temporizador para ejecutar de nuevo el ataque
    private float timer;

    protected override void Attack()
    {
        if (player == null) return; //Si no hay jugador, se corta

        //Calcula la distancia al jugador
        float distance = Vector2.Distance(transform.position, player.position);
        //Si esta en rango, ataca
        if (range >=distance)
        {
            timer += Time.deltaTime;
  
            //Si ya acabo el cooldown, aplica el ataque
            if (timer >= cooldown)
            {
                anim.SetTrigger("attack");

                player.GetComponent<PlayerHealthController>().TakeDamage(enemyType.GetAttackDamage);

                timer = 0f;
            }
        }
    }
}