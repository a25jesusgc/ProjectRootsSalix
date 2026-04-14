using System;
using UnityEngine;

[Serializable]
public class PlayerFertilizer
{
    // Tipo de fertilizante
    [SerializeField] private FertilizerType type;

    // Cantidad de fertilizante en unidades
    [SerializeField] private int amount;

    public PlayerFertilizer(FertilizerType type, int amount){
        this.type = type;
        this.amount = amount;
    }
    
    public FertilizerType GetFertilizerType => type;
    public int GetFertilizerAmount => amount;

    public void ChangeAmount(int value)
    {
        amount += value;
    }
    
}
