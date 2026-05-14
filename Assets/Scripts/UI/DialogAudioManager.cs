using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAudioManager : MonoBehaviour
{
    private const double START_SAMPLE = 0;
    private const double END_SAMPLE = 3800;

    private AudioSource source;

    private bool playingAudio;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueSystem.instance.IsWrittingText && !playingAudio)
        {
            StartCoroutine(PlayAudioCoroutine());
        }
    }

    private IEnumerator PlayAudioCoroutine()
    {
        playingAudio = true;

        source.pitch = 0.75f;
        source.Play();
        source.timeSamples = 0;
        source.SetScheduledEndTime(AudioSettings.dspTime + END_SAMPLE / source.clip.frequency);

        while (DialogueSystem.instance.IsWrittingText)
        {
            if (!source.isPlaying)
            {
                source.Play();
                source.timeSamples = (int) START_SAMPLE;
                source.SetScheduledEndTime(AudioSettings.dspTime + (double)(END_SAMPLE - START_SAMPLE) / source.clip.frequency);
            }
            yield return null;
        }

        if (source.isPlaying) source.SetScheduledEndTime(source.clip.samples);

        playingAudio = false;
    }
}
