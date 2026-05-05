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
                if(!enemy.isDefeated) enemy.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (EnemyController enemy in enemies)
            {
                enemy.transform.position = enemy.originPos;
                enemy.gameObject.SetActive(false);
            }
        }
    }
}
