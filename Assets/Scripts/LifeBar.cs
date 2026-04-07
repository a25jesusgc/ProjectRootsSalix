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

    private RectTransform rectTransform;

    private const float BAR_RESIZE_SPEED = 200f;
    private const float BAR_SPEED = 1f;
    private const float BAR_SIZE_MULT = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(player.GetMaxHP * BAR_SIZE_MULT, rectTransform.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 healthBarSizeTarget = new Vector2(player.GetMaxHP * BAR_SIZE_MULT, rectTransform.sizeDelta.y);
        rectTransform.sizeDelta = Vector2.MoveTowards(rectTransform.sizeDelta, healthBarSizeTarget, Time.deltaTime * BAR_RESIZE_SPEED);

        // Porcentaje de vida actual del jugador
        float healthPercent = player.GetHealthPercentage;

        // Actualiza el tamaño de la barra de vida según la vida actual del jugador
        fillLifeBar.fillAmount = Mathf.MoveTowards(fillLifeBar.fillAmount, healthPercent, Time.deltaTime * BAR_SPEED);

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
