using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneLoader : MonoBehaviour
{
    public static ZoneLoader instance;

    [SerializeField] private TransitionController transitionController;
    [SerializeField] private Transform player;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(InitialLoad());
    }

    public void ChangeZone(string unload, string load, Vector3 targetPos)
    {
        StartCoroutine(ChangeZoneCoroutine(unload, load, targetPos));
    }

    private IEnumerator ChangeZoneCoroutine(string unload, string load, Vector3 targetPos)
    {
        GlobalUtils.pause = true;

        AudioManager.instance.MuteMusic(true);
        transitionController.PlayTransition(false);
        yield return new WaitUntil(() => transitionController.transitionFinished);

        player.position = targetPos;

        SceneManager.UnloadSceneAsync(unload);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);
        
        AudioManager.instance.MuteMusic(false);

        transitionController.PlayTransition(true);
        yield return new WaitUntil(() => transitionController.transitionFinished);

        GlobalUtils.pause = false;
    }

    private IEnumerator InitialLoad()
    {
        GlobalUtils.pause = true;
        
        string scene = PlayerData.GetInstance.GetCheckpoint != null ? PlayerData.GetInstance.GetCheckpoint.GetScene : "L0Cave";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);

        transitionController.PlayTransition(true);
        yield return new WaitUntil(() => transitionController.transitionFinished);

        GlobalUtils.pause = false;
    }
}
