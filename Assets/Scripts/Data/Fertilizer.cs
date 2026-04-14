using UnityEngine;

[CreateAssetMenu(fileName = "Fertilizer", menuName = "Custom/Crear tipo de fertilizante")]
public class Fertilizer : ScriptableObject
{
    // Tipo de fertilizante
    [SerializeField] private FertilizerType type;

    // Prefab de la bala que instancia con este fertilizante
    [SerializeField] private GameObject projectilePrefab;

    // Icono del fertilizante
    [SerializeField] private Sprite fertilizerIcon;


    public FertilizerType GetFertilizerType => type;
    public GameObject GetProjectile => projectilePrefab;
    public Sprite GetIcon => fertilizerIcon;
}

public enum FertilizerType
{
    AUTOAIM,
    PIERCING,
    BIG
}