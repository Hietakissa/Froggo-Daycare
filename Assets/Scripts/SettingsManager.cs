using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using HietakissaUtils;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [SerializeField] TMP_InputField sensitivityInputField;
    [SerializeField] Slider sensitivitySlider;

    [SerializeField] TMP_InputField masterVolumeInputField;
    [SerializeField] Slider masterVolumeSlider;

    [SerializeField] TMP_InputField musicVolumeInputField;
    [SerializeField] Slider musicVolumeSlider;

    //[SerializeField] AudioMixerGroup masterVolumeGroup;
    //[SerializeField] AudioMixerGroup musicVolumeGroup;
    [SerializeField] AudioMixer mixer;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadSettings();
    }

    public void SaveSettings()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor) return;

        Debug.Log("saving settings");
        Serializer.Save(PlayerData.sensitivity.ToString(), "sensitivity");
        Serializer.Save(PlayerData.masterVolume.ToString(), "masterVolume");
        Serializer.Save(PlayerData.musicVolume.ToString(), "musicVolume");
    }

    void LoadSettings()
    {
        //Debug.Log($"Load settings, is editor {Application.isEditor}, should apply defaults: {PlayerData.sensitivity == 0f && (Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor)}");

        if (PlayerData.settingsSet) return;

        if (PlayerData.sensitivity == 0f && (Application.platform == RuntimePlatform.WebGLPlayer || Application.isEditor))
        {
            Debug.Log("Sensitivity was 0, applying default settings");

            PlayerData.sensitivity = 1f;
            PlayerData.masterVolume = 80;
            PlayerData.musicVolume = 55;
        }
        else
        {
            Debug.Log("loading settings");

            if (Serializer.SaveDataExists("sensitivity")) PlayerData.sensitivity = float.Parse(Serializer.Load("sensitivity"));
            else PlayerData.sensitivity = 1f;

            if (Serializer.SaveDataExists("masterVolume")) PlayerData.masterVolume = int.Parse(Serializer.Load("masterVolume"));
            else PlayerData.masterVolume = 80;

            if (Serializer.SaveDataExists("musicVolume")) PlayerData.musicVolume = int.Parse(Serializer.Load("musicVolume"));
            else PlayerData.musicVolume = 55;
        }

        PlayerData.settingsSet = true;

        //PlayerData.sensitivity = 1f;
        UpdateSensitivity();
        UpdateMasterVolume();
        UpdateMusicVolume();
    }

    public void SensitivityInputFieldChanged(string value)
    {
        if (float.TryParse(value.Replace(".", ","), out float floatValue))
        {
            PlayerData.sensitivity = floatValue;
            UpdateSensitivity();
        }
    }

    public void SensitivitySliderChanged(float value)
    {
        PlayerData.sensitivity = value;
        UpdateSensitivity();
    }
    void UpdateSensitivity()
    {
        //slider.value = PlayerData.sensitivity;
        sensitivitySlider.SetValueWithoutNotify(PlayerData.sensitivity);
        sensitivityInputField.text = PlayerData.sensitivity.RoundToDecimalPlaces(2).ToString();
    }

    public void MasterVolumeInputFieldChanged(string value)
    {
        if (int.TryParse(value.Replace(".", ","), out int intValue))
        {
            PlayerData.masterVolume = intValue;
            UpdateMasterVolume();
        }
    }

    public void MasterVolumeSliderChanged(float value)
    {
        PlayerData.masterVolume = (int)value;
        UpdateMasterVolume();
    }

    void UpdateMasterVolume()
    {
        //slider.value = PlayerData.sensitivity;
        masterVolumeSlider.SetValueWithoutNotify(PlayerData.masterVolume);
        masterVolumeInputField.text = PlayerData.masterVolume.ToString();

        mixer.SetFloat("MasterVolume", Maf.ReMap(0, 100, -80, 20, PlayerData.masterVolume));
    }

    public void MusicVolumeInputFieldChanged(string value)
    {
        if (int.TryParse(value.Replace(".", ","), out int intValue))
        {
            PlayerData.musicVolume = intValue;
            UpdateMusicVolume();
        }
    }

    public void MusicVolumeSliderChanged(float value)
    {
        PlayerData.musicVolume = (int)value;
        UpdateMusicVolume();
    }

    void UpdateMusicVolume()
    {
        //slider.value = PlayerData.sensitivity;
        musicVolumeSlider.SetValueWithoutNotify(PlayerData.musicVolume);
        musicVolumeInputField.text = PlayerData.musicVolume.ToString();

        Debug.Log($"Setting music volume to {Maf.ReMap(0, 100, -80, 20, PlayerData.musicVolume)}");
        mixer.SetFloat("MusicVolume", Maf.ReMap(0, 100, -80, 20, PlayerData.musicVolume));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Quit Game!");
#else
        Application.Quit();
#endif
    }

    public static void Restart()
    {
        GameManager.IsPaused = false;
        SceneManager.LoadScene(0);
    }
}
