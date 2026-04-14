using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponSelection : MonoBehaviour
{
    [SerializeField] private List<Sprite> weapons;
    [SerializeField] private Image weaponIcon;

    public void UpdateSelectedWeaponIcon(int index)
    {
        weaponIcon.sprite = weapons[index];
    }
}
