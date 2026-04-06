using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    // Elementos de la UI
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueContinueIcon;

    // Bandera de si el texto esta siendo escrito
    private bool isWrittingText;

    // Variable auxiliar para establecer el texto al componente de UI
    private string writtingText;
    
    // Bandera de si hay dialogo en proceso
    private bool dialogueOpen;
    public bool IsDialogueOpen => dialogueOpen;

    // Índice del diálogo que está siendo mostrado
    private int dialogueIndex;

    // Lista de diálogos
    private List<Dialogue> currentDialogues;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }


    void LateUpdate()
    {
        if (dialogueOpen && Input.GetKeyDown(KeyCode.Z))
        {
            ProgressDialogue();
        }

        dialogueContinueIcon.SetActive(!isWrittingText);
    }

    public void ShowDialogue(List<Dialogue> dialogues)
    {
        if(dialogues == null || dialogues.Count < 1) return;
        dialogueOpen = true;
        dialogueIndex = 0;
        currentDialogues = dialogues;
        dialogueText.text = "";
        dialogueBox.SetActive(true);
    }

    public void ProgressDialogue()
    {
        if (isWrittingText)
        {
            CompleteDialogueText();
        }
        else
        {
            if(dialogueIndex < currentDialogues.Count)
            {
                PrintLine();
            }
            else
            {
                HideDialogue();
            }
        }
    }

    private void PrintLine()
    {
        string text = currentDialogues[dialogueIndex].GetText;
        writtingText = text;
        StartCoroutine(WriteTextCoroutine(text));
    }

    private IEnumerator WriteTextCoroutine(string text)
    {
        isWrittingText = true;
        dialogueText.text = "";

        bool skipLetter = false;

        foreach (char c in text)
        {
            // For HTML tags
            if(c == '<')
            {
                skipLetter = true;
            }
            if (c == '>')
            {
                skipLetter = false;
            }

            dialogueText.text += c.ToString();

            if (c.ToString() == " " || skipLetter)
            {
                continue;
            }

            yield return new WaitForSeconds(0.1f);
        }
        isWrittingText = false;
        dialogueIndex++;
    }

    public void CompleteDialogueText()
    {
        StopAllCoroutines();
        
        dialogueText.text = writtingText;
        isWrittingText = false;
        dialogueIndex++;
    }

    public void HideDialogue()
    {
        StartCoroutine(CloseDialogueCoroutine());
    }
    private IEnumerator CloseDialogueCoroutine()
    {
        dialogueBox.SetActive(false);
        isWrittingText = false;

        yield return new WaitForSeconds(0.1f);

        dialogueOpen = false;
    }

    public void SetDialogueScale(Vector3 scale)
    {
        dialogueBox.transform.localScale = scale;
    }
}
