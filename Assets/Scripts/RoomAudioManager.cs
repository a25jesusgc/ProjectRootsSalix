using UnityEngine;


// Componente para hacer que props solo suenen mientras se esté dentro de su zona
public class RoomAudioManager : MonoBehaviour
{
    private AudioSource[] props;

    void Start()
    {
        props = GetComponentsInChildren<AudioSource>();
        Debug.Log("FOUND " + props.Length);
    }

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
