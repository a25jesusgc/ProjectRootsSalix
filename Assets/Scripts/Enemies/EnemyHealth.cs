using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Tipo de enemigo, objeto que contiene sus stats
    [SerializeField] private Enemy enemyType;
    [SerializeField] private bool destroyOnDefeat = true;
    private EnemyController enemyController;
    private SpriteRenderer spriteRenderer;

    // Vida actual
    private int currentHP;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Inicializa su vida
        currentHP = enemyType.GetHealth;
    }

    // Función de recibir daño
    public void ReceiveDamage(int damage, DamageType damageType)
    {
        // Calcula el daño recibido teniendo en cuenta las resistencias
        int receivedDamage = Mathf.RoundToInt(damage * enemyType.GetResistances[(int)damageType]);
        if (receivedDamage <= 0) receivedDamage = 1;
        currentHP -= receivedDamage;
        ShowDamagedFeedback();
        if (currentHP <= 0)
        {
            currentHP = 0;
            Defeat();
        }
    }

    // Función de gestión de cuando la vida del enemigo llega a 0 y cae derrotado
    public void Defeat()
    {
        if(enemyController != null) enemyController.SetDefeated();
        if(destroyOnDefeat) Destroy(gameObject, 1f);
    }

    private void ShowDamagedFeedback()
    {
        StopCoroutine("DamagedFeedbackCoroutine");
        StartCoroutine("DamagedFeedbackCoroutine");
    }

    private IEnumerator DamagedFeedbackCoroutine()
    {
        float time = 0f;
        while (time < 0.2f)
        {
            time += Time.deltaTime;

            float value = Mathf.Lerp(0.75f, 1f, time / 0.2f);
            spriteRenderer.color = new Color(1f, value, value, 1f);

            yield return null;
        }
    }

    public float GetHealthPercentage => (float)currentHP / enemyType.GetHealth;
    public float GetDrainEffectiveness => enemyType.GetResistances[(int) DamageType.THORN];
}
