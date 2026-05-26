using System.Collections.Generic;
using UnityEngine;

public class TextInteractable : Interactable
{
    [SerializeField] private List<Dialogue> dialogues;
    
    public override void OnInteract(Transform player)
    {
        DialogueSystem.instance.ShowDialogue(dialogues);
    }
}
