using System.Collections.Generic;
using UnityEngine;

public class RoomEnemiesManager : MonoBehaviour
{
    [SerializeField] private List<EnemyController> enemies;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (EnemyController enemy in enemies)
            {
                if(!enemy.isDefeated){
                    enemy.gameObject.SetActive(true);
                    enemy.currentState = EnemyController.EnemyState.Idle;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (EnemyController enemy in enemies)
            {
                if(!enemy.isDefeated && enemy.TryGetComponent(out EnemyHealth enemyHealth)) enemyHealth.ResetEnemy(); 
                enemy.gameObject.SetActive(false);
            }
        }
    }
}
