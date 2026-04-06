using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Custom/Crear tipo de enemigo")]
public class Enemy : ScriptableObject
{
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

    public int GetHealth => health;
    public int GetBodyDamage => bodyDamage;
    public int GetAttackDamage => attackDamage;
    public float GetMoveSpeed => movSpeed;
    public float[] GetResistances => resistances;
}
