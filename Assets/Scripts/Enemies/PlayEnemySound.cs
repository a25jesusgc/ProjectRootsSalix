using UnityEngine;

public class PlayEnemySound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] soundEffects;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(int index){
        audioSource.clip = soundEffects[index];
        audioSource.Play();
    }
}
