using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject icon;

    public void ShowInteractionIcon(bool value)
    {
        if(icon != null) icon.SetActive(value);
    }
    public abstract void OnInteract();
}
