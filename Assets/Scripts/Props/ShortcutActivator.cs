using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class ShortcutActivator : Interactable
{
    [SerializeField] Collider2D[] colliderList;
    [SerializeField] GameObject bridge;

    public string shortcutId;

    void Start()
    {
      if(PlayerData.GetInstance.WasEventCompleted(shortcutId))
        {
            ActivateShortcut();
        } 
    }

    public override void OnInteract(Transform player)
    {
        if (!PlayerData.GetInstance.WasEventCompleted(shortcutId))
        {
            ActivateShortcut();
            PlayerData.GetInstance.CompleteEvent(shortcutId);            
        }
    }

    void ActivateShortcut()
    {
        bridge.GetComponent<SpriteRenderer>().enabled=true;

        foreach (Collider2D col in colliderList)
        {
            col.enabled=false;
        }
    }
}
