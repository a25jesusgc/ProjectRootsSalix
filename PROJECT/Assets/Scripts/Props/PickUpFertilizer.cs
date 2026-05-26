using UnityEngine;

public class PickUpFertilizer : PickUpItem
{
    [SerializeField] private FertilizerType fertilizerType;
    [SerializeField] private int amount;

    public override void OnPickup(Collider2D collision)
    {
        PlayerData.GetInstance.AddFertilizer(new PlayerFertilizer(fertilizerType, amount));
    }
}
