using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour
{
    private Transform window;
    [SerializeField] private List<Enemy> enemies;
    [SerializeField] private EnemyDataPanel enemyData;

    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        window = transform.GetChild(0);
        index = 0;
        enemyData.LoadEnemyData(enemies[index]);
    }

    public void ShowPreviousEnemy()
    {
        if(index > 0)
        {
            index--;
        }
        else
        {
            index = enemies.Count - 1;
        }
        enemyData.LoadEnemyData(enemies[index]);
    }

    public void ShowNextEnemy()
    {
        if(index < enemies.Count - 1)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        enemyData.LoadEnemyData(enemies[index]);
    }

}
