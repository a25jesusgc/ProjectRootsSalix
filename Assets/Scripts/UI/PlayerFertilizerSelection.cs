using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFertilizerSelection : MonoBehaviour
{
    [SerializeField] private GameObject fertilizerBox;
    [SerializeField] private Image fertilizerIcon;
    [SerializeField] private TextMeshProUGUI fertilizerAmount;
    [SerializeField] private Sprite defaultBulletIcon;

    void Start()
    {
        UpdateSelectedFertilizerIconAndAmount(null, 0);
    }

    public void ShowFertilizerBox(bool value)
    {
        fertilizerBox.SetActive(value);
    }

    public void UpdateSelectedFertilizerIconAndAmount(Sprite icon, int amount)
    {
        fertilizerIcon.sprite = icon != null ? icon : defaultBulletIcon;
        fertilizerAmount.text = amount > 0 ? amount.ToString() : "";
    }
}
