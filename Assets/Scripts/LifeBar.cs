using UnityEngine.UI;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    // Referencia al componente Image que representa la barra de vida
    [SerializeField] private Image fillLifeBar;

    // Sprites de Vida
    [SerializeField] private Sprite fullLife;
    [SerializeField] private Sprite mediumLife;
    [SerializeField] private Sprite lowLife;
    [SerializeField] private PlayerHealthController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Porcentaje de vida actual del jugador
        float healthPercent = player.GetHealthPercentage;

        // Actualiza el tamaño de la barra de vida según la vida actual del jugador
        fillLifeBar.fillAmount = healthPercent;

        // Cambia el sprite de la barra de vida según el porcentaje de vida restante
        if (healthPercent > 0.5f)
        {
            fillLifeBar.sprite = fullLife;
        }
        else if (healthPercent > 0.25f)
        {
            fillLifeBar.sprite = mediumLife;
        }
        else
        {
            fillLifeBar.sprite = lowLife;
        }
    }
}
