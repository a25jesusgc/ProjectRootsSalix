using System.Collections.Generic;
using UnityEngine;

public class RootInteractable : Interactable
{
    [SerializeField] private List<Dialogue> dialogues;
    
    public override void OnInteract(Transform player)
    {
        DialogueSystem.instance.ShowDialogue(dialogues, false, true);
    }
}
