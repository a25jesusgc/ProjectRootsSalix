using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject icon;

    public bool showIcon = true;

    public void ShowInteractionIcon(bool value)
    {
        if(icon != null) icon.SetActive(value && showIcon);
    }
    public abstract void OnInteract(Transform player);
}
