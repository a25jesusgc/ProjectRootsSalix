using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Tipo de enemigo, objeto que contiene sus stats
    [SerializeField] private Enemy enemyType;
    private EnemyController enemyController;

    // Vida actual
    private int currentHP;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
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
        Destroy(gameObject, 1f);
    }

    public float GetHealthPercentage => (float)currentHP / enemyType.GetHealth;
    public float GetDrainEffectiveness => enemyType.GetResistances[(int) DamageType.THORN];
}
