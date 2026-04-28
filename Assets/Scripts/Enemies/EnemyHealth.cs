using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Tipo de enemigo, objeto que contiene sus stats
    [SerializeField] private Enemy enemyType;
    public bool deactivateOnDefeat = true;
    private EnemyController enemyController;
    private SpriteRenderer spriteRenderer;

    // Vida actual
    private int currentHP;

    private Vector3 startPos;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Inicializa su vida
        currentHP = enemyType.GetHealth;
        startPos = transform.position;
    }

    // Función de recibir daño
    public void ReceiveDamage(int damage, DamageType damageType)
    {
        // Calcula el daño recibido teniendo en cuenta las resistencias
        int receivedDamage = Mathf.RoundToInt(damage * enemyType.GetResistances[(int)damageType] / GetDayCycleDefenseMultiplier());
        Debug.Log("Enemy "+ gameObject.name + " received " + receivedDamage + " damage.");
        if (receivedDamage <= 0) receivedDamage = 1;
        currentHP -= receivedDamage;
        ShowDamagedFeedback();
        if (currentHP <= 0)
        {
            currentHP = 0;
            Defeat();
        }
    }

    private float GetDayCycleDefenseMultiplier()
    {
        float multiplier = 1f;

        if (DayCycleManager.instance.IsDay)
        {
            return enemyType.GetDayDefenseMultipler;
        }
        else if (DayCycleManager.instance.IsNight)
        {
            return enemyType.GetNightDefenseMultipler;
        }

        return multiplier;
    }

    // Función de gestión de cuando la vida del enemigo llega a 0 y cae derrotado
    public void Defeat()
    {
        PlayerData.GetInstance.DefeatEnemy(enemyType.GetEnemyType);
        if(enemyController != null) enemyController.SetDefeated();
        if(deactivateOnDefeat) Invoke("Deactivate", 1f);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
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

            float value = Mathf.Lerp(0.65f, 1f, time / 0.2f);
            spriteRenderer.color = new Color(1f, value, value, 1f);

            yield return null;
        }
    }

    public void Revive()
    {
        transform.position = startPos;
        currentHP = enemyType.GetHealth;
        if(enemyController != null) enemyController.SetDefeated(false);
    }

    public float GetHealthPercentage => (float)currentHP / enemyType.GetHealth;
    public float GetDrainEffectiveness => enemyType.GetResistances[(int) DamageType.THORN];
}
