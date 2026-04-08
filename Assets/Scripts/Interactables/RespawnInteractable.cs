using System.Collections.Generic;
using UnityEngine;

public class RespawnInteractable : Interactable
{
    [SerializeField] private Transform respawnPoint;

    // Al interactuar con un punto de respawn, se establece la ubicación y se guarda la partida
    public override void OnInteract()
    {
        PlayerData.GetInstance.SetRepawnPoint(respawnPoint.position);
        PlayerData.Save();
    }
}
