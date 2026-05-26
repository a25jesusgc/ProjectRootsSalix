using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Checkpoint", menuName = "Custom/Crear checkpoint")]
[Serializable]
public class Checkpoint : ScriptableObject
{
    // ID del checkpoint
    [SerializeField] private string zoneID;
    // Punto de respawn
    [SerializeField] private Vector3 position;
    // Nombre de la escena a la que pertenece (para cargarla)
    [SerializeField] private string scene;

    public string GetZoneID => zoneID;
    public string GetName => GlobalUtils.GetLocalizedString("Zones", zoneID);
    public Vector3 GetPosition => position;
    public string GetScene => scene;
}
