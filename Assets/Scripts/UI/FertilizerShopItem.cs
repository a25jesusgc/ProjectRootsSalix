using UnityEngine;

public class FertilizerShopItem : MonoBehaviour
{
    [SerializeField] private FertilizerType fertilizerType;
    [SerializeField] private int amount;
    [SerializeField] private int price;

    public void BuyFertilizer()
    {
        if (price <= PlayerData.GetInstance.GetCurrency)
        {
            PlayerData.GetInstance.AddFertilizer(new PlayerFertilizer(fertilizerType, amount));
            PlayerData.GetInstance.ChangeCurrency(-price);
            PlayerCurrency.instance.ChangeCurrency(-price);
        }
    }
}
