using UnityEngine;

public class ShopInteractable : Interactable
{

    [SerializeField] private GameObject shopWindow;

    public override void OnInteract(Transform player)
    {
        ShowShop(true);
    }

    public void ShowShop(bool value)
    {
        shopWindow.SetActive(value);
        PlayerCurrency.instance.forceShow = value;
        GlobalUtils.pause = value;
    }
}
