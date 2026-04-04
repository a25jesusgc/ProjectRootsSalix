using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider audioGeneralSlider;
    [SerializeField] private Slider audioMusicSlider;
    [SerializeField] private Slider audioSFXSlider;
    [SerializeField] private TextMeshProUGUI audioGeneralValue;
    [SerializeField] private TextMeshProUGUI audioMusicValue;
    [SerializeField] private TextMeshProUGUI audioSFXValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioGeneralSlider.value =  PlayerPrefs.GetInt(SettingConstants.VolumeGeneral, 50);
        audioMusicSlider.value = PlayerPrefs.GetInt(SettingConstants.VolumeMusic, 100);
        audioSFXSlider.value = PlayerPrefs.GetInt(SettingConstants.VolumeSFX, 100);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeGeneralVolume(float value)
    {
        PlayerPrefs.SetInt(SettingConstants.VolumeGeneral, Mathf.RoundToInt(value));
        AudioManager.instance.UpdateMixerVolume();
        audioGeneralValue.text = value.ToString("F0");
    }
    public void ChangeMusicVolume(float value)
    {
        PlayerPrefs.SetInt(SettingConstants.VolumeMusic, Mathf.RoundToInt(value));
        AudioManager.instance.UpdateMixerVolume();
        audioMusicValue.text = value.ToString("F0");
    }
    public void ChangeSFXVolume(float value)
    {
        PlayerPrefs.SetInt(SettingConstants.VolumeSFX, Mathf.RoundToInt(value));
        AudioManager.instance.UpdateMixerVolume();
        audioSFXValue.text = value.ToString("F0");
    }
}
