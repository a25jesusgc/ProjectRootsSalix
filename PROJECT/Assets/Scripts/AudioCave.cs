using UnityEngine;

public class AudioCave : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource.Play();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
