using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleDoor : MonoBehaviour
{
    [SerializeField] private List<GameObject> activatedProps;
    [SerializeField] private UnityEvent onAllActivated;
    [SerializeField] private UnityEvent onNotActivated;
    private bool disableNotActivation;

    public void CheckAllActivated()
    {
        bool allActivated = true;
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
        if (allActivated)
        {
            if (onAllActivated != null) onAllActivated.Invoke();
        }
        else
        {
            if (!disableNotActivation && onNotActivated != null) onNotActivated.Invoke();
        }
    }
    public void SetDisableNotActivation(bool value)
    {
        disableNotActivation = value;
    }

}
