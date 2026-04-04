using UnityEngine.UI;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    // Referencia al componente Image que representa la barra de vida
    public Image fillLifeBar;
    private PlayerController player;
    private float maxLife;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Busca el componente PlayerController en el objeto llamado "Player" y lo asigna a la variable player
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        maxLife = player.life;

    }

    // Update is called once per frame
    void Update()
    {
        // Actualiza el tamaño de la barra de vida según la vida actual del jugador
        fillLifeBar.fillAmount = player.life / maxLife;
    }
}
