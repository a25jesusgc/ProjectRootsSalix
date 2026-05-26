using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu;
    [SerializeField] private GameObject journalWindow;
    [SerializeField] private GameObject settingsWindow;
    public bool isPaused = false;

    [SerializeField] private PlayerInput playerInput;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Detectar si se presiona la tecla de pausa (Escape)
        if (playerInput.actions["Pause"].triggered)
        {
            // Si el juego ya está pausado, reanudarlo
            if (isPaused)
            {

                ResumeGame();
            }
            // Si el juego no está pausado, pausarlo
            else if (!GlobalUtils.pause)
            {
                Pause();
            }
        }
    }

    // Método para reanudar el juego
    public void ResumeGame()
    {
        // Desactivar el menú de pausa y reanudar el tiempo
        GlobalUtils.pause = false;
        Settings(false);
        Journal(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        CursorManager.instance.SetCursorCrosshair();
    }

    // Método para pausar el juego
    public void Pause()
    {
        // Activar el menú de pausa y detener el tiempo
        GlobalUtils.pause = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        CursorManager.instance.SetCursorArrow();
    }


    public void Settings(bool value)
    {
        // Activa o desactiva la ventana de configuración
        settingsWindow.SetActive(value);
    }
    public void Journal(bool value)
    {
        // Activa o desactiva la ventana de diario
        journalWindow.SetActive(value);
    }

    public void CloseSettings()
    {
        settingsWindow.SetActive(false);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }

}
