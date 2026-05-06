using System.Collections;
using UnityEngine;


public class DoorTeleport : MonoBehaviour
{
    public Transform target;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
    }

    // Cuando el jugador entra en la puerta, se pausa el juego, se reproduce la transición, 
    // se teleporta al jugador, se reproduce la transición de salida y luego se reanuda el juego.
    IEnumerator TeleportPlayer(Transform player)
    {
        GlobalUtils.pause = true;
        audioSource.Play();
        TransitionController.instance.PlayTransition(false);
        yield return new WaitUntil(() => TransitionController.instance.transitionFinished);
        player.position = target.position;
        TransitionController.instance.PlayTransition(true);
        yield return new WaitUntil(() => TransitionController.instance.transitionFinished);
        GlobalUtils.pause = false;
    }
}