using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponSelection : MonoBehaviour
{
    [SerializeField] private List<Sprite> weapons;
    [SerializeField] private Image icon;

    public void UpdateSelectedWeaponIcon(int index)
    {
        icon.sprite = weapons[index];
    }
}
