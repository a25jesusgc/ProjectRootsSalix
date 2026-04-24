using NUnit.Framework;
using UnityEngine;

// Componente que permite la instanciación de drops al llamar al método público SpawnDrops
public class DropSpawner : MonoBehaviour
{

    // Probabilidad de que se dropee loot
    [SerializeField] private float dropChance;

    // Drops que puede soltar
    [SerializeField] private GameObject[] drops;
    // Probabilidades de cada drop
    [SerializeField] private float[] dropTypeChances;

    // Mínima y máxima cantidad de drops que puede soltar
    [SerializeField] private int minDropAmount;
    [SerializeField] private int maxDropAmount;

    public void SpawnDrops()
    {
        // Drops y DropTypeChances han de ser de la misma longitud
        Assert.AreEqual(drops.Length, dropTypeChances.Length, "La cantidad de drops y la cantidad de probabilidades para los drops han de ser arrays de la misma longitud.");
        
        // Vemos si suelta drops
        int roll = Random.Range(0, 100);
        if (roll <= dropChance)
        {
            // Si suelta drops, calculamos aleatoriamente la cantidad de drops a soltar
            int amount = Random.Range(minDropAmount, maxDropAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                // Por cada drop a soltar, calculamos aleatóriamente el tipo de drop
                for(int j = 0; j < dropTypeChances.Length; j++)
                {
                    // Por cada tipo de drop, vemos si se elige ese tipo según la probabilidad
                    // (Importante: se comprueba si es ya el último elemento de la lista.
                    // Esto se hacer porque, si el resto de tipos de drops no han sido elegidos
                    // por probabilidad, entonces como ya sólo queda 1 tipo posible, pues tendrá
                    // que ser ese. Esto implica que su prob. será 100% - % de los anteriores.)
                    int dropTypeRoll = j == dropTypeChances.Length -1 ? 0 : Random.Range(0, 100);
                    if (dropTypeRoll <= dropTypeChances[j])
                    {
                        // Si este tipo de drop ha sido seleccionado por la probabilidad, lo instanciamos
                        Instantiate(drops[j], transform.position, Quaternion.identity);
                        break;
                    }
                }
            }
        }
    }
}
