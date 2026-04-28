using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Custom/Crear tipo de enemigo")]
public class Enemy : ScriptableObject
{
    // Identificador del tipo de enemigo
    [SerializeField] private string enemyType;

    // Cantidad de vida del enemigo, cuanto daño puede aguantar
    [SerializeField] private int health;

    // El daño que inflinge al jugador si entra en contacto físico
    [SerializeField] private int bodyDamage;

    // El daño que inflinge con sus ataques
    [SerializeField] private int attackDamage;

    // Velocidad de movimiento
    [SerializeField] private float movSpeed;

    // Lista de resistencias para cada tipo de daño
    [SerializeField] private float[] resistances;

    // Multiplicadores según etapa del día
    [SerializeField] private float dayAttackMultipler = 1f;
    [SerializeField] private float nightAttackMultipler = 1f;
    [SerializeField] private float dayDefenseMultipler = 1f;
    [SerializeField] private float nightDefenseMultipler = 1f;

    public string GetEnemyType => enemyType;
    public int GetHealth => health;
    public int GetBodyDamage => bodyDamage;
    public int GetAttackDamage => attackDamage;
    public float GetMoveSpeed => movSpeed;
    public float[] GetResistances => resistances;
    public float GetDayAttackMultipler => dayAttackMultipler;
    public float GetNightAttackMultipler => nightAttackMultipler;
    public float GetDayDefenseMultipler => dayDefenseMultipler;
    public float GetNightDefenseMultipler => nightDefenseMultipler;
}

[Serializable] 
public class EnemyDefeated
{
    [SerializeField] private string enemyType;
    [SerializeField] private int defeatCount;

    public EnemyDefeated(string enemyType) {
        this.enemyType = enemyType;
        defeatCount = 1;
    }

    public string GetEnemyType => enemyType;
    public int GetDefeatCount => defeatCount;

    public void DefeatEnemy()
    {
        defeatCount++;
    }

}