using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private AudioLoop music;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayMusic(music, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        // TODO empezar partida nueva
        Debug.Log("Start New Game");
    }

    public void Continue()
    {
        // TODO continuar partida guardada
        Debug.Log("Continue");
    }

    public void Settings(bool value)
    {
        // Activa o desactiva la ventana de configuración
        settingsWindow.SetActive(value);
    }

    public void ExitGame()
    {
        // Cierra la aplicación
        Application.Quit();
    }
}
