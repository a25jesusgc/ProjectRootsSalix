using UnityEngine;

public class PropAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] props;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (AudioSource audioSource in props)
            {
                audioSource.volume = 1f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (AudioSource audioSource in props)
            {
                audioSource.volume = 0f;
            }
        }
    }
}
