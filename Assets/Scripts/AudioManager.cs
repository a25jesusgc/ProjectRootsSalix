using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup generalMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup sfxMixerGroup;

    public AudioSource music;

    private bool isChangingClip;
    private bool mute;
    private bool customLoop;
    private float muteValue;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateMixerVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChangingClip)
        {
            music.volume = Mathf.MoveTowards(music.volume, mute ? muteValue : 1, Time.deltaTime);
        }
    }

    public void PlayMusic(AudioLoop theme, bool instant = false)
    {
        if (instant)
        {
            StartCoroutine(PlayMusicCoroutine(theme));
        }
        else
        {
            StopCoroutine("PlayMusicFadeCoroutine");
            StartCoroutine("PlayMusicFadeCoroutine", theme); 
        }
    }

    private IEnumerator PlayMusicCoroutine(AudioLoop theme)
    {
        int loopStart = theme.GetStartLoopSample;
        int loopEnd = theme.GetEndLoopSample;
        customLoop = loopEnd > 0;

        music.Stop();
        music.clip = theme.GetAudioClip;
        music.Play();

        music.timeSamples = 0;

        if(customLoop)
        {
            music.SetScheduledEndTime(AudioSettings.dspTime + (double)loopEnd / music.clip.frequency);

            while (customLoop)
            {

                if (!music.isPlaying)
                {
                    music.Play();
                    music.timeSamples = loopStart;
                    music.SetScheduledEndTime(AudioSettings.dspTime + (double)(loopEnd - loopStart) / music.clip.frequency);
                }


                yield return null;
            }
        } 
    }

    private IEnumerator PlayMusicFadeCoroutine(AudioLoop theme)
    {
        isChangingClip = true;
        while (music.volume > 0)
        {
            music.volume = Mathf.MoveTowards(music.volume, 0, Time.deltaTime);
            yield return null;
        }
        isChangingClip = false;
        customLoop = false;
        StartCoroutine(PlayMusicCoroutine(theme));
    }

    public void MuteMusic(bool isMuted, float value = 0f, bool instant = false)
    {
        mute = isMuted;
        muteValue = value;
        if (instant)
        {
            music.volume = mute ? muteValue : 1;
        }
    }

    public void UpdateMixerVolume()
    {
        generalMixerGroup.audioMixer.SetFloat("GeneralVolume", Mathf.Log10((PlayerPrefs.GetInt(SettingConstants.VolumeGeneral, 50) + 1) / 100f) * 40);
        musicMixerGroup.audioMixer.SetFloat("MusicVolume", Mathf.Log10((PlayerPrefs.GetInt(SettingConstants.VolumeMusic, 100) + 1) / 100f) * 40);
        sfxMixerGroup.audioMixer.SetFloat("SFXVolume", Mathf.Log10((PlayerPrefs.GetInt(SettingConstants.VolumeSFX, 100) + 1) / 100f) * 40);
    }


}
