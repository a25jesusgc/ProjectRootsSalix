using System.Collections.Generic;
using UnityEngine;

public class RespawnInteractable : Interactable
{
    [SerializeField] private Checkpoint checkpoint;
    [SerializeField] private GameObject travelMenu;
    [SerializeField] private List<Checkpoint> checkpointList;
    [SerializeField] private List<GameObject> travelButtonsList;

    private int availableTPs;

    void Start()
    {
        UpdateAvailableTravelPoints();
    }

    // Al interactuar con un punto de respawn, se recupera la vida, se establece el checkpoint y se guarda la partida
    public override void OnInteract(Transform player)
    {
        player.GetComponent<PlayerHealthController>().FullRecovery();

        if(!PlayerData.GetInstance.WasCheckpointDiscovered(checkpoint)) PlayerData.GetInstance.DiscoverCheckpoint(checkpoint);
        PlayerData.GetInstance.SetCheckpoint(checkpoint);
        PlayerData.Save();

        ShowTravelMenu(true);
    }

    public void ShowTravelMenu(bool show)
    {
        if(availableTPs == 0) return;
        travelMenu.SetActive(show);
        GlobalUtils.pause = show;
    }

    private void UpdateAvailableTravelPoints()
    {
        availableTPs = 0;
        for (int i = 0; i < checkpointList.Count; i++)
        {
            bool canTravel = PlayerData.GetInstance.WasCheckpointDiscovered(checkpointList[i]) && checkpointList[i].GetID != checkpoint.GetID;
            if(canTravel) availableTPs++;
            travelButtonsList[i].SetActive(canTravel);
        }
    }
}
