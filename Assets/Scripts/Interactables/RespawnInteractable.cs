using System.Collections.Generic;
using UnityEngine;

public class RespawnInteractable : Interactable
{
    [SerializeField] private Transform respawnPoint;

    public override void OnInteract()
    {
        PlayerData.GetInstance.SetRepawnPoint(respawnPoint.position);
        PlayerData.Save();
    }
}
