using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGauntlet : MonoBehaviour
{
    [SerializeField] private string eventID;
    [SerializeField] private List<GauntletWave> enemyWaves;
    [SerializeField] private List<GameObject> pathBlocking;

    private bool started;

    void Start()
    {
        if(PlayerData.GetInstance.WasEventCompleted(eventID)) Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(started || PlayerData.GetInstance.WasEventCompleted(eventID)) return;
        if (collision.CompareTag("Player"))
        {
            StartGauntlet();
        }
    }

    private void StartGauntlet()
    {
        StartCoroutine(GauntletCoroutine());
    }

    private IEnumerator GauntletCoroutine()
    {
        started = true;
        GlobalUtils.pause = true;
        foreach (GameObject pathBlock in pathBlocking)
        {
            pathBlock.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);

        GlobalUtils.pause = false;

        foreach (GauntletWave wave in enemyWaves)
        {
            foreach (EnemyController enemy in wave.GetEnemies)
            {
                enemy.gameObject.SetActive(true);
            }

            yield return new WaitUntil(() => wave.GetEnemies.Count((e) => !e.isDefeated) == 0);

            yield return new WaitForSeconds(0.5f);
        }

        foreach (GameObject pathBlock in pathBlocking)
        {
            pathBlock.SetActive(false);
        }
        PlayerData.GetInstance.CompleteEvent(eventID);
        started = false;
    }
}

[Serializable]
public class GauntletWave
{
    [SerializeField] private List<EnemyController> enemies;

    public List<EnemyController> GetEnemies => enemies;
}