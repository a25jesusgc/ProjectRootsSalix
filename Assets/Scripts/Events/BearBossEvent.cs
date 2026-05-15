using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearBossEvent : MonoBehaviour
{
    [SerializeField] private Animator bossAnim;
    [SerializeField] private BearLumberjackBoss boss;
    [SerializeField] private List<Dialogue> preBattleDialogues;
    [SerializeField] private List<Dialogue> postBattleDialogues;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private AudioLoop forestTheme;
    [SerializeField] private AudioLoop battleTheme;
    [SerializeField] private GameObject barriers;
    [SerializeField] private GameObject root;

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

        // El oso está talando raíz
        bossAnim.SetBool("cutting_tree", true);

        CameraController.instance.SetTrackingTarget(boss.transform);

        yield return new WaitForSeconds(1f);

        // Corta la raíz
        root.SetActive(false);
        
        // Se da cuenta del jugador y se gira hacia él
        bossAnim.SetBool("cutting_tree", false);
        yield return new WaitForSeconds(0.5f);
        bossAnim.SetFloat("x", 1);

        yield return new WaitForSeconds(1f);

        // Diálogos del boss antes de pelear
        DialogueSystem.instance.ShowDialogue(preBattleDialogues, true);
        yield return new WaitUntil(() => !DialogueSystem.instance.IsDialogueOpen);
        GlobalUtils.pause = true;

        AudioManager.instance.MuteMusic(false, 0f, true);
        AudioManager.instance.PlayMusic(battleTheme, true);

        // Se devuelve el enfoque de la cámara al jugador y se activa la barra de vida
        CameraController.instance.ResetTrackingTarget();
        healthBar.SetActive(true);
        barriers.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        // Empieza la batalla
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
        bossAnim.Play("Tired");

        AudioManager.instance.MuteMusic(true);

        // Diálogos finales del boss
        DialogueSystem.instance.ShowDialogue(postBattleDialogues, true);
        yield return new WaitUntil(() => !DialogueSystem.instance.IsDialogueOpen);
        GlobalUtils.pause = true;

        bossAnim.SetBool("is_defeated", true);

        // Se devuelve el juego a la normalidad con la música del bosque
        AudioManager.instance.MuteMusic(false);
        AudioManager.instance.PlayMusic(forestTheme);
        GlobalUtils.pause = false;
    }
}
