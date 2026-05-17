using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaWolfBattleEvent : MonoBehaviour
{
    [SerializeField] private Animator bossAnim;
    [SerializeField] private AlphaWolfBoss boss;
    [SerializeField] private List<Dialogue> preBattleDialogues;
    [SerializeField] private List<Dialogue> postBattleDialogues;
    [SerializeField] private Transform battlePosition;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private AudioLoop forestTheme;
    [SerializeField] private AudioLoop battleTheme;
    [SerializeField] private GameObject barriers;

    private bool battleStarted;

    void Start()
    {
        if(PlayerData.GetInstance.GetEnemyDefeatCount(boss.enemy.GetEnemyType) > 0) Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!battleStarted) Battle(collision.transform);
        }
    }

    private void Battle(Transform player)
    {
        battleStarted = true;
        StartCoroutine(BattleCoroutine(player));
    }

    private IEnumerator BattleCoroutine(Transform player)
    {
        // Se pausa el juego, se silencia la música y se enfoca al boss
        GlobalUtils.pause = true;
        AudioManager.instance.MuteMusic(true);

        CameraController.instance.SetTrackingTarget(boss.transform);

        yield return new WaitForSeconds(0.5f);

        // Diálogos del boss antes de pelear
        DialogueSystem.instance.ShowDialogue(preBattleDialogues, true);
        yield return new WaitUntil(() => !DialogueSystem.instance.IsDialogueOpen);
        GlobalUtils.pause = true;


        // El boss salta hacia la arena
        bossAnim.SetTrigger("jump");

        yield return new WaitForSeconds(1f);

        boss.transform.position = battlePosition.position;

        yield return new WaitForSeconds(1f);

        // El boss aulla amenazante, empezando la música de batalla

        bossAnim.SetBool("howling", true);
        AudioManager.instance.MuteMusic(false, 0f, true);
        AudioManager.instance.PlayMusic(battleTheme, true);

        yield return new WaitForSeconds(1f);

        // Se devuelve el enfoque de la cámara al jugador y se activa la barra de vida
        CameraController.instance.ResetTrackingTarget();
        healthBar.SetActive(true);
        barriers.SetActive(true);

        yield return new WaitForSeconds(2f);

        // Deja de aullar, se establece su objetivo como el jugador y empieza la batalla
        bossAnim.SetBool("howling", false);
        boss.SetPlayer(player);
        GlobalUtils.pause = false;

        // Se espera aque el boss sea derrotado
        yield return new WaitUntil(() => boss.isDefeated);

        // Se detiene el juego, se desactiva la barra de vida y se silencia la música
        GlobalUtils.pause = true;

        healthBar.SetActive(false);
        barriers.SetActive(false);

        bossAnim.SetFloat("x", 0);
        bossAnim.SetFloat("y", -1);
        bossAnim.SetBool("is_tired", true);

        AudioManager.instance.MuteMusic(true);

        // Diálogos finales del boss
        DialogueSystem.instance.ShowDialogue(postBattleDialogues, true);
        yield return new WaitUntil(() => !DialogueSystem.instance.IsDialogueOpen);
        GlobalUtils.pause = true;

        bossAnim.SetBool("is_tired", false);

        // Se devuelve el juego a la normalidad con la música del bosque
        AudioManager.instance.MuteMusic(false);
        AudioManager.instance.PlayMusic(forestTheme);
        GlobalUtils.pause = false;
        
        PlayerData.Save();
        TransitionController.instance.ShowGameSavedNotification();
    }
}
