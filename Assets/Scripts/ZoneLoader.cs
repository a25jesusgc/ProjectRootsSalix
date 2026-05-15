using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

// Singleton que gestiona la carga y transiciones entre zonas
public class ZoneLoader : MonoBehaviour
{
    public static ZoneLoader instance;

    // Transición de la UI para fundido a negro
    [SerializeField] private TransitionController transitionController;

    // Referencia al jugador
    [SerializeField] private Transform player;

    // Lista de los checkpoints
    public List<Checkpoint> checkpoints;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        // Al entrar al juego, carga la zona en la que estaba el jugador
        StartCoroutine(InitialLoad());
    }

    // Metodo para cambiar de zona
    public void ChangeZone(string unload, string load, Vector3 targetPos)
    {
        StartCoroutine(ChangeZoneCoroutine(unload, load, targetPos));
    }

    private IEnumerator ChangeZoneCoroutine(string unload, string load, Vector3 targetPos)
    {
        // Pausa, silencia la música y hace fundido a negro
        GlobalUtils.pause = true;

        AudioManager.instance.MuteMusic(true);
        transitionController.PlayTransition(false);
        yield return new WaitUntil(() => transitionController.transitionFinished);

        // Transporta al jugador a la posición objetivo
        player.position = targetPos;


        // Descarga la escena del nivel (en principio, el nivel actual)
        SceneManager.UnloadSceneAsync(unload);

        // Carga la escena del nivel al que accede
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(load, LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);
        
        // Una vez cargado el nivel, el juego continua
        AudioManager.instance.MuteMusic(false);

        transitionController.PlayTransition(true);
        yield return new WaitUntil(() => transitionController.transitionFinished);

        GlobalUtils.pause = false;
    }

    private IEnumerator InitialLoad()
    {
        GlobalUtils.pause = true;

        PlayerData.ReloadInstance();
        
        // Obtiene el checkpoint del jugador para cargar su escena y colocar ahí al jugador
        // En caso de que sea null, se empieza en la cueva
        string scene = PlayerData.GetInstance.GetCheckpointID != null ? GetCheckpoint(PlayerData.GetInstance.GetCheckpointID).GetScene : "L0Cave";

        // Carga aditiva de la escena en cuestión
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);

        // Una vez ha terminado de cargarla, el juego empieza
        transitionController.PlayTransition(true);
        yield return new WaitUntil(() => transitionController.transitionFinished);

        GlobalUtils.pause = false;
    }

    public Checkpoint GetCheckpoint(string checkpointID)
    {
        return checkpoints.First((c) => c.GetZoneID == checkpointID);
    }
}
