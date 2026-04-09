using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    // Vida máxima que puede tener el jugador
    private const int BASE_HP = 200;
    private const int LIFE_UPGRADE_AMOUNT = 15;
    private int maxHP;
    // Vida actual
    private int currentHP;
    private Animator anim;
    // Propiedad para obtener valores de vida
    public int GetMaxHP => maxHP;
    public float GetHealthPercentage => (float)currentHP / maxHP;

    private const float INVULNERABILITY_DURATION = 0.5f;
    private bool invulnerable;

    private SpriteRenderer sr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHP = BASE_HP + LIFE_UPGRADE_AMOUNT * PlayerData.GetInstance.GetLifeUpgrades;
        currentHP = maxHP;
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Función para curar al jugador
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
    }

    // Función para recibir daño
    public void TakeDamage(int damage, bool selfInflicted = false)
    {
        if(invulnerable) return;
        currentHP -= damage;
        if (!selfInflicted)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(InvulnerabilityCoroutine());
        }
        if (currentHP <= 0)
        {
            currentHP = 0;
            RestartGame();
        }
    }

    // Funcion para cuando obtiene mejora de vida
    public void LifeUpgrade()
    {
        PlayerData.GetInstance.GotLifeUpgrade();
        maxHP = BASE_HP + LIFE_UPGRADE_AMOUNT * PlayerData.GetInstance.GetLifeUpgrades;
        currentHP = maxHP;
    }

    void RestartGame()
    {
        GetComponent<PlayerController>().transform.position = PlayerData.GetInstance.GetRespawn;
        currentHP = maxHP;
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        invulnerable = true;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);

        yield return new WaitForSeconds(INVULNERABILITY_DURATION);

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        invulnerable = false;
    }
}
