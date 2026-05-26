using TMPro;
using UnityEngine;

public class FastTravel : MonoBehaviour
{
    [SerializeField] private Checkpoint checkpoint;
    [SerializeField] private TextMeshProUGUI zoneName;

    void Start()
    {
        zoneName.text = checkpoint.GetName;
    }

    // Método para realizar el viaje rápido
    public void Travel()
    {
        // Se realiza el cambio de zona, cargando la nueva escena, descargando la actual y transportando al jugador a la posición del checkpoint
        ZoneLoader.instance.ChangeZone(ZoneLoader.instance.GetCheckpoint(PlayerData.GetInstance.GetCheckpointID).GetScene, checkpoint.GetScene, checkpoint.GetPosition);
        
        // Se devuelve el cursor a modo crosshair
        CursorManager.instance.SetCursorCrosshair();

        // Se establece el checkpoint al que se viaja como último checkpoint visitado y se guarda la partida
        PlayerData.GetInstance.SetCheckpoint(checkpoint);
        PlayerData.Save();
    }
}
