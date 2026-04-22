using UnityEngine;

public class FertilizerShopItem : MonoBehaviour
{
    [SerializeField] private FertilizerType fertilizerType;
    [SerializeField] private int amount;
    [SerializeField] private int price;

    // Al darle a comprar fertilizante
    public void BuyFertilizer()
    {
        // Se comprueba que tenga dinero suficiente
        if (price <= PlayerData.GetInstance.GetCurrency)
        {
            // Se añade el fertilizante y se resta el precio al dinero del jugador
            PlayerData.GetInstance.AddFertilizer(new PlayerFertilizer(fertilizerType, amount));
            PlayerData.GetInstance.ChangeCurrency(-price);
            PlayerCurrency.instance.ChangeCurrency(-price);
        }
    }
}
