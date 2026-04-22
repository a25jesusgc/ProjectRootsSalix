using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [SerializeField] private float dropChance;
    [SerializeField] private GameObject[] drops;
    [SerializeField] private float[] dropTypeChances;
    [SerializeField] private int minDropAmount;
    [SerializeField] private int maxDropAmount;

    public void SpawnDrops()
    {
        int roll = Random.Range(0, 100);
        if (roll <= dropChance)
        {
            int amount = Random.Range(minDropAmount, maxDropAmount);

            for (int i = 0; i < amount; i++)
            {
                for(int j = dropTypeChances.Length -1 ; j >= 0; j--)
                {
                    int dropTypeRoll = Random.Range(0, 100);
                    if (dropTypeRoll <= dropTypeChances[j])
                    {
                        Instantiate(drops[j], transform.position, Quaternion.identity);
                    }
                }
            }
        }
    }
}
