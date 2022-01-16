using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuGO;
    [SerializeField] private GameObject OptionsGO;

    [SerializeField] private Slider masterVolume_Slider;
    [SerializeField] private Slider musicVolume_Slider;
    [SerializeField] private Slider SFXVolume_Slider;
    [SerializeField] private Slider Sensitivity_Slider;

    private void Start()
    {
        LoadOptionMenuValues();
    }

    private void LoadOptionMenuValues()
    {
        masterVolume_Slider.value = AudioManager.Instance.MasterVolume;
        musicVolume_Slider.value = AudioManager.Instance.MusicVolume;
        SFXVolume_Slider.value = AudioManager.Instance.SFXVolume;
        Sensitivity_Slider.value = StaticData.Sensitivity;
    }

    public void ToggleOptionsMenu()
    {
        bool state = MainMenuGO.activeInHierarchy;
        MainMenuGO.SetActive(!state);
        OptionsGO.SetActive(state);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void MasterVolumeChanged(float value)
    {
        AudioManager.Instance.MasterVolume = value;
    }

    public void MusicVolumeChanged(float value)
    {
        AudioManager.Instance.MusicVolume = value;
    }

    public void SFXChanged(float value)
    {
        AudioManager.Instance.SFXVolume = value;
    }

    public void SensitivityChanged(float value)
    {
        StaticData.Sensitivity = value;
    }
}
