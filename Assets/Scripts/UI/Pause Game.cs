using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused = false;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Detectar si se presiona la tecla de pausa (Escape)
        if (Input.GetKeyDown(KeyCode.Escape))
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

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

    }

    // Método para pausar el juego
    public void Pause()
    {
        // Activar el menú de pausa y detener el tiempo
        GlobalUtils.pause = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }
}
