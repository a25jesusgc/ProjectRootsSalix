using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Dialogue
{
    [SerializeField] private string text;

    public string GetText => text;

    public Dialogue(string text)
    {
        this.text = text;
    }
}

[Serializable]
public class DialogueList
{
    [SerializeField] private List<Dialogue> dialogues;
    public List<Dialogue> GetDialogues => dialogues;

}