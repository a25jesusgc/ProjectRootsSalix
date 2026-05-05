using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;

    // Para los Inputs del jugador
    [SerializeField] private PlayerInput playerInput;

    // Elementos de la UI
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialogueContinueIcon;

    // Componente imagen de la pantalla oscura para oscurecer el juego cuando se quiera mostrar solo el dialogo, como en las raices.
    [SerializeField] private Image blackScreenImage;
    //Referencias a las corrutinas
    private Coroutine WriteTextReference;
    private Coroutine CloseDialogueReference;

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
        if (dialogueOpen && playerInput.actions["Interact"].triggered)
        {
            ProgressDialogue();
        }

        dialogueContinueIcon.SetActive(!isWrittingText);
    }

    public void ShowDialogue(List<Dialogue> dialogues, bool startAlready = false, bool darkBackground = false)
    {
        if(dialogues == null || dialogues.Count < 1) return;
        GlobalUtils.pause = true;
        dialogueOpen = true;
        dialogueIndex = 0;
        currentDialogues = dialogues;
        dialogueText.text = "";
        dialogueBox.SetActive(true);

        //Activa el oscurecimiento si se indica
        if (darkBackground)
            StartCoroutine(BlackScreenCoroutine(true));

        if(startAlready) ProgressDialogue();
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
        WriteTextReference= StartCoroutine(WriteTextCoroutine(text));
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
        //StopAllCoroutines();

        //Paramos la corrutina que escribe el texto y la de cierre del dialogo, pero la de oscurecer no
        if(WriteTextReference!=null){
            StopCoroutine(WriteTextReference);
            WriteTextReference=null;
        }

        if(CloseDialogueReference!=null){
            StopCoroutine(CloseDialogueReference);
            CloseDialogueReference=null;
        }
        
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
        GlobalUtils.pause = false;

        //Elimina la pantalla oscura
        StartCoroutine(BlackScreenCoroutine(false));
    }

    public void SetDialogueScale(Vector3 scale)
    {
        dialogueBox.transform.localScale = scale;
    }

    private IEnumerator BlackScreenCoroutine(bool show)
    {
        float time = 0f;
        
        while (time < 1f)
        {
            time += Time.deltaTime*3;

            float origin = show ? 0f : 1f;
            float target = show ? 1f : 0f;
            blackScreenImage.color = new Color(0f, 0f, 0f, Mathf.Lerp(origin, target, time / 1f));

            yield return null;
        }
    }
}
