using System.Collections.Generic;
using UnityEngine;

public class RespawnInteractable : Interactable
{
    [SerializeField] private Checkpoint checkpoint;
    [SerializeField] private GameObject travelMenu;
    [SerializeField] private List<GameObject> travelButtonsList;

    private int availableTPs;

    void Start()
    {
        UpdateAvailableTravelPoints();
    }

    // Al interactuar con un punto de respawn, se recupera la vida, se establece el checkpoint y se guarda la partida
    public override void OnInteract(Transform player)
    {
        // Curación del jugador
        player.GetComponent<PlayerHealthController>().FullRecovery();

        // Si es un checkpoint nuevo, se guarda su descubrimiento
        if(!PlayerData.GetInstance.WasCheckpointDiscovered(checkpoint)) PlayerData.GetInstance.DiscoverCheckpoint(checkpoint);
        
        // Se establece como el último checkpoint visitado y se guarda la partida
        PlayerData.GetInstance.SetCheckpoint(checkpoint);
        PlayerData.Save();

        // Se abre el panel de viaje rápido
        ShowTravelMenu(true);
    }

    public void ShowTravelMenu(bool show)
    {
        // Si no tiene ningún otro destino disponible, no muestra el panel
        if(availableTPs == 0) return;

        // Muestra el panel y pausa el juego
        travelMenu.SetActive(show);
        GlobalUtils.pause = show;
    }


    // Función para obtener los puntos de viaje disponibles
    private void UpdateAvailableTravelPoints()
    {
        availableTPs = 0;

        // Se recorre la lista de checkpoints
        for (int i = 0; i < ZoneLoader.instance.checkpoints.Count; i++)
        {
            // Por cada checkpoint, se establece que puede viajar a él si lo ha descubierto y no es el checkpoint en el que se está ahora mismo
            bool canTravel = PlayerData.GetInstance.WasCheckpointDiscovered(ZoneLoader.instance.checkpoints[i]) && ZoneLoader.instance.checkpoints[i].GetZoneID != checkpoint.GetZoneID;
            if(canTravel) availableTPs++;

            // Activa el botón o no de ese destino si es un destino válido (disponible y no es el actual)
            travelButtonsList[i].SetActive(canTravel);
        }
    }
}
