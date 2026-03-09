using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private bool loopMusic;
    private AudioSource musicSource;

    public AudioLoop music;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        
    }
    
    public void PlayMusic(AudioLoop music)
    {
        musicSource.Stop();
        musicSource.clip = music.GetAudioClip;
        LoopMusic(music);
    }

    public void LoopMusic(AudioLoop loop)
    {
        StartCoroutine(LoopMusicCoroutine(loop.GetStartLoopSample, loop.GetEndLoopSample));
    }

    private IEnumerator LoopMusicCoroutine(int loopStart, int loopEnd)
    {
        loopMusic = true;

        yield return new WaitUntil(() => musicSource.clip != null);

        musicSource.Play();
        musicSource.timeSamples = 0;
        musicSource.SetScheduledEndTime(AudioSettings.dspTime + (double)loopEnd / musicSource.clip.frequency);

        while (loopMusic)
        {
            if (!musicSource.isPlaying)
            {
                musicSource.Play();
                musicSource.timeSamples = loopStart;
                musicSource.SetScheduledEndTime(AudioSettings.dspTime + (double)(loopEnd - loopStart) / musicSource.clip.frequency);
            }
            yield return null;
        }

    }
}
