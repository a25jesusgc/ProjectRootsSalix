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

        // Abre la ventana de tienda y pausa el juego
        shopWindow.SetActive(value);

        // Obliga a mostrar en la UI el dinero del jugador
        PlayerCurrency.instance.forceShow = value;
        GlobalUtils.pause = value;
    }
}
