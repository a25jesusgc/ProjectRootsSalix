using System.Linq;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private PlayerWeapon[] availableWeapons;
    private PlayerWeapon currentWeapon;
    private int currentWeaponIndex;

    void Start()
    {
        currentWeaponIndex = 0;
        UpdateSelectedWeapon(currentWeaponIndex);
    }

    // Selecciona el arma anterior
    public void SelectPreviousWeapon()
    {   
        
        // Actualiza el índice del arma
        if (currentWeaponIndex > 0)
        {
            currentWeaponIndex -= 1;
        }
        else
        {
            currentWeaponIndex = availableWeapons.Count() - 1;
        }
        // Actualiza el arma seleccionada con el nuevo arma según el índice
        UpdateSelectedWeapon(currentWeaponIndex);
    }

    // Selecciona el arma siguiente
    public void SelectNextWeapon()
    {
        // Actualiza el índice del arma
        if (currentWeaponIndex < availableWeapons.Length - 1)
        {
            currentWeaponIndex += 1;
        } else
        {
            currentWeaponIndex = 0;
        }
        // Actualiza el arma seleccionada con el nuevo arma según el índice
        UpdateSelectedWeapon(currentWeaponIndex);
    }

    // Actualiza el arma seleccionada según el índice
    public void UpdateSelectedWeapon(int index)
    {
        currentWeapon = availableWeapons[index];
    }

    // Dispara el arma seleccionada
    public void ShootCurrentWeapon()
    {
        currentWeapon.Shoot();
    }
}