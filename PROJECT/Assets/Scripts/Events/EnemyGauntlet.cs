using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyGauntlet : MonoBehaviour
{
    [SerializeField] private string eventID;
    [SerializeField] private List<GauntletWave> enemyWaves;
    [SerializeField] private GameObject pathBlocking;
    [SerializeField] private AudioLoop battleTheme;
    [SerializeField] private AudioLoop areaTheme;

    private AudioSource audioSource;


    private bool started;

    void Start()
    {
        if(PlayerData.GetInstance.WasEventCompleted(eventID)) Destroy(gameObject);
        audioSource = GetComponent<AudioSource>();
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
        pathBlocking.SetActive(true);
        audioSource.Play();

        AudioManager.instance.PlayMusic(battleTheme, true);

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

        pathBlocking.SetActive(false);
        audioSource.Play();
        PlayerData.GetInstance.CompleteEvent(eventID);
        started = false;

        AudioManager.instance.PlayMusic(areaTheme);
        
        PlayerData.Save();
        TransitionController.instance.ShowGameSavedNotification();
    }
}

[Serializable]
public class GauntletWave
{
    [SerializeField] private List<EnemyController> enemies;

    public List<EnemyController> GetEnemies => enemies;
}