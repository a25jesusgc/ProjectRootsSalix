using Unity.VisualScripting;
using UnityEngine;

public class FertilizerSelector : MonoBehaviour
{
    [SerializeField] private PlayerCannon playerCannon;

    private int currentFertilizerIndex;

    private bool useFertilizer;


    // Selecciona el fertilizante anterior
    public void SelectPreviousFertilizer()
    {
        if (PlayerData.GetInstance.GetPlayerFertilizers.Count == 0)
        {
            SetEmptyFertilizer();
            return;
        }

        if (useFertilizer && currentFertilizerIndex == 0)
        {
            SetEmptyFertilizer();
            return;
        }

        if (!useFertilizer)
        {
            currentFertilizerIndex = PlayerData.GetInstance.GetPlayerFertilizers.Count - 1;
        } else
        {
            currentFertilizerIndex -= 1;
        }
        useFertilizer = true;
        UpdateSelectedFertilizer(currentFertilizerIndex);
    }

    // Selecciona el fertilizante siguiente
    public void SelectNextFertilizer()
    {
        if (PlayerData.GetInstance.GetPlayerFertilizers.Count == 0)
        {
            SetEmptyFertilizer();
            return;
        }

        if (useFertilizer && currentFertilizerIndex == PlayerData.GetInstance.GetPlayerFertilizers.Count - 1)
        {
            SetEmptyFertilizer();
            return;
        }
        
        
        if (!useFertilizer)
        {
            currentFertilizerIndex = 0;
        }else
        {
            currentFertilizerIndex += 1;
        }
        useFertilizer = true;
        UpdateSelectedFertilizer(currentFertilizerIndex);
    }

    // Actualiza el fertilizante seleccionado según el índice
    public void UpdateSelectedFertilizer(int index)
    {
        playerCannon.SetCurrentFertilizer(useFertilizer ? PlayerData.GetInstance.GetPlayerFertilizers[index] : null);
    }

    public void SetEmptyFertilizer()
    {
        useFertilizer = false;
        currentFertilizerIndex = 0;
        UpdateSelectedFertilizer(currentFertilizerIndex);
    }
}
