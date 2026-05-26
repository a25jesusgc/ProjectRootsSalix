using UnityEngine;

public class PlayAreaMusic : MonoBehaviour
{
    [SerializeField] private AudioLoop theme;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlayMusic(theme);
        }
    }
}
