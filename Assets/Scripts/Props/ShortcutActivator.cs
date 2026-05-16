using UnityEngine;

public class ShortcutActivator : Interactable
{
    [SerializeField] private Collider2D[] colliderList;
    [SerializeField] private GameObject bridge;
    private Animator anim;
    private AudioSource sound;

    public string shortcutId;

    void Start()
    {
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();

        if(PlayerData.GetInstance.WasEventCompleted(shortcutId))
        {
            ActivateShortcut();
        } 
    }

    public override void OnInteract(Transform player)
    {
        if (!PlayerData.GetInstance.WasEventCompleted(shortcutId))
        {
            ActivateShortcut();
            PlayerData.GetInstance.CompleteEvent(shortcutId);
            sound.Play();
        }
    }

    void ActivateShortcut()
    {
        bridge.GetComponent<SpriteRenderer>().enabled = true;

        foreach (Collider2D col in colliderList)
        {
            col.enabled = false;
        }

        anim.SetTrigger("activate");

        showIcon = false;
    }
}
