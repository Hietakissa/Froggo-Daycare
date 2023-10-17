using UnityEngine.SceneManagement;
using HietakissaUtils;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [SerializeField] TMP_InputField sensitivityInputField;
    [SerializeField] Slider sensitivitySlider;

    void Awake()
    {
        Instance = this;
        LoadSettings();
    }

    public void SaveSettings()
    {
#if !UNITY_EDITOR
        Serializer.Save(PlayerData.sensitivity.ToString(), "sensitivity");
#endif
    }

    void LoadSettings()
    {
        //loading not implemented
#if UNITY_EDITOR
        PlayerData.sensitivity = 1f;
#else
        if (Serializer.SaveDataExists("sensitivity")) PlayerData.sensitivity = float.Parse(Serializer.Load("sensitivity"));
        else PlayerData.sensitivity = 1f;
#endif

        //PlayerData.sensitivity = 1f;
        UpdateSensitivity();
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
