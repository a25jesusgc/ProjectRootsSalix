using System.Collections.Generic;
using UnityEngine;

public class TextInteractable : Interactable
{
    [SerializeField] private List<Dialogue> dialogues;
    
    public override void OnInteract()
    {
        DialogueSystem.instance.ShowDialogue(dialogues);
    }
}
