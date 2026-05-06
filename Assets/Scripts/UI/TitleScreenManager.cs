using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject newGameWindow;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private Button continueButton;
    [SerializeField] private AudioLoop music;

    private bool hasSaveFile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.instance.PlayMusic(music);
        
        // Comprueba si existe partida guardada
        hasSaveFile = File.Exists(GlobalUtils.SAVE_PATH);
        continueButton.interactable = hasSaveFile;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Selección de botón de nueva partida
    public void NewGameSelected()
    {
        
        // Si ya existe guardado, pide confirmación
        if (hasSaveFile)
        {
            ShowNewGameWarning(true);
        }
        else
        {
            // Si no hay partida guardada, inicia partida sin confirmación
            NewGame();
        }
    }

    // Muestra u oculta la ventana de confirmación de nueva partida
    public void ShowNewGameWarning(bool show)
    {
        newGameWindow.SetActive(show);
    }

    // Inicia una nueva partida
    public void NewGame()
    {
        PlayerData.ResetInstance();
        SceneManager.LoadScene("WorldScene");
    }

    // Continua la partida guardada
    public void Continue()
    {
        SceneManager.LoadScene("WorldScene");
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
