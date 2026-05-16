using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleDoor : MonoBehaviour
{
    // Identificador para guardar que el jugador haya completado el puzzle
    [SerializeField] private string puzzleID;

    // Lista de props que contienen la interfaz IActivatedProp requeridos con Activate para que el puzzle se complete
    [SerializeField] private List<GameObject> activatedProps;

    // Acción a ejecutar cuando todos los props están activados
    [SerializeField] private UnityEvent onAllActivated;

    // Acción a ejecutar cuando no todos los props están activados
    [SerializeField] private UnityEvent onNotActivated;

    // Desactiva la ejecución de onNotActivated
    private bool disableNotActivation;

    public string GetPuzzleID => puzzleID;

    void Start()
    {
        if (PlayerData.GetInstance.WasEventCompleted(puzzleID))
        {
            if (onAllActivated != null) onAllActivated.Invoke();
        }
    }


    // Comprueba si todos los props están activados
    public bool CheckAllActivated()
    {
        bool allActivated = true;
        // Comprobamos todos los props, y si alguno no está activado, se cambia allActivated a false
        foreach(GameObject prop in activatedProps)
        {
            if(prop.TryGetComponent(out IActivatedProp activatedProp))
            {
                if (!activatedProp.Activated())
                {
                    allActivated = false;
                    break;
                }
            }
            else
            {
                allActivated = false;
                break;
            }
        }
        
        // Si están activados, ejecuta onAllActivated
        if (allActivated)
        {
            if (onAllActivated != null) onAllActivated.Invoke();
            if(!PlayerData.GetInstance.WasEventCompleted(puzzleID)) PlayerData.GetInstance.CompleteEvent(puzzleID);
        }
        // Si no lo están, y no se ha desactivado, se ejecuta onNotActivated
        else
        {
            if (!disableNotActivation && onNotActivated != null) onNotActivated.Invoke();
        }
        return allActivated;
    }
    
    // Función para gestionar externamente la desactivación de onNotActivated
    public void SetDisableNotActivation(bool value)
    {
        disableNotActivation = value;
    }

}
