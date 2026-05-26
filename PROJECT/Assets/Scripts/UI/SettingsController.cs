using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider audioGeneralSlider;
    [SerializeField] private Slider audioMusicSlider;
    [SerializeField] private Slider audioSFXSlider;

    [SerializeField] private TextMeshProUGUI audioGeneralValue;
    [SerializeField] private TextMeshProUGUI audioMusicValue;
    [SerializeField] private TextMeshProUGUI audioSFXValue;

    [SerializeField] private Toggle cameraShakeToggle;

    void Start()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[PlayerPrefs.GetInt("lang", LocalizationSettings.SelectedLocale.SortOrder)];

        audioGeneralSlider.value =  PlayerPrefs.GetInt(SettingConstants.VolumeGeneral, 50);
        audioMusicSlider.value = PlayerPrefs.GetInt(SettingConstants.VolumeMusic, 100);
        audioSFXSlider.value = PlayerPrefs.GetInt(SettingConstants.VolumeSFX, 100);

        audioGeneralValue.text = audioGeneralSlider.value.ToString("F0");
        audioMusicValue.text = audioMusicSlider.value.ToString("F0");
        audioSFXValue.text = audioSFXSlider.value.ToString("F0");

        cameraShakeToggle.isOn = PlayerPrefs.GetInt(SettingConstants.CameraShake, 1) == 1;
    }

    void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void ChangeGeneralVolume(float value)
    {
        PlayerPrefs.SetInt(SettingConstants.VolumeGeneral, Mathf.RoundToInt(value));
        audioGeneralValue.text = value.ToString("F0");
        AudioManager.instance.UpdateMixerVolume();
    }
    public void ChangeMusicVolume(float value)
    {
        PlayerPrefs.SetInt(SettingConstants.VolumeMusic, Mathf.RoundToInt(value));
        audioMusicValue.text = value.ToString("F0");
        AudioManager.instance.UpdateMixerVolume();
    }
    public void ChangeSFXVolume(float value)
    {
        PlayerPrefs.SetInt(SettingConstants.VolumeSFX, Mathf.RoundToInt(value));
        audioSFXValue.text = value.ToString("F0");
        AudioManager.instance.UpdateMixerVolume();
    }
    public void ChangeLocale(string locale)
    {
        switch (locale)
        {
            case "en":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                PlayerPrefs.SetInt("lang", 0);
                break;
            case "es":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                PlayerPrefs.SetInt("lang", 1);
                break;
            case "gl":
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
                PlayerPrefs.SetInt("lang", 2);
                break;
        }
    }
    public void ToggleCamShake(bool value)
    {
        PlayerPrefs.SetInt(SettingConstants.CameraShake, value ? 1 : 0);
    }
}
