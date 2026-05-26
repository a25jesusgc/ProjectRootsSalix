using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndDemo : MonoBehaviour
{
    public bool restartGame;

    void Start()
    {
        if (restartGame)
        {
            Invoke("RestartGame", 3f);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(EndDemoCoroutine());
        }
    }

    private IEnumerator EndDemoCoroutine()
    {
        GlobalUtils.pause = true;
        
        TransitionController.instance.PlayTransition(false);
        yield return new WaitUntil(() => TransitionController.instance.transitionFinished);
        
        PlayerData.Save();

        yield return new WaitForSeconds(1f);

        GlobalUtils.pause = false;

        SceneManager.LoadScene("EndDemo");
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
