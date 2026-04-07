using UnityEngine;

public class AmbienceSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;
    [SerializeField] private float minWait;
    [SerializeField] private float maxWait;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float minVolume;
    [SerializeField] private float maxVolume;
    [Range(-1, 1)]
    [SerializeField] private float stereoRange;

    private AudioSource audioSource;

    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timer = Random.Range(minWait, maxWait);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = Random.Range(minWait, maxWait);
            audioSource.clip = sounds[Random.Range(0, sounds.Length)];
            audioSource.volume = Random.Range(minVolume, maxVolume);
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.panStereo = Random.Range(-stereoRange, stereoRange);
            audioSource.Play();
        }
    }
}
