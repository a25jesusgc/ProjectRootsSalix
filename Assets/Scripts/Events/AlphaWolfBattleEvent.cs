using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaWolfBattleEvent : MonoBehaviour
{
    [SerializeField] private Animator bossAnim;
    [SerializeField] private AlphaWolfBoss boss;
    [SerializeField] private List<Dialogue> preBattleDialogues;
    [SerializeField] private Transform battlePosition;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private AudioLoop battleTheme;

    private bool battleStarted;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!battleStarted) StartBattle(collision.transform);
        }
    }

    private void StartBattle(Transform player)
    {
        battleStarted = true;
        StartCoroutine(StartBattleCoroutine(player));
    }

    private IEnumerator StartBattleCoroutine(Transform player)
    {
        GlobalUtils.pause = true;

        CameraController.instance.SetTrackingTarget(boss.transform);

        yield return new WaitForSeconds(0.5f);

        DialogueSystem.instance.ShowDialogue(preBattleDialogues, true);
        yield return new WaitUntil(() => !DialogueSystem.instance.IsDialogueOpen);
        GlobalUtils.pause = true;

        bossAnim.SetTrigger("jump");

        yield return new WaitForSeconds(1f);

        boss.transform.position = battlePosition.position;

        yield return new WaitForSeconds(1f);

        bossAnim.SetBool("howling", true);
        AudioManager.instance.PlayMusic(battleTheme, true);

        yield return new WaitForSeconds(1f);

        CameraController.instance.ResetTrackingTarget();

        yield return new WaitForSeconds(2f);

        bossAnim.SetBool("howling", false);
        healthBar.SetActive(true);
        boss.SetPlayer(player);
        GlobalUtils.pause = false;
    }
}
