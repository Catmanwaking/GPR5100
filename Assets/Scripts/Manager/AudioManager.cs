using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public static AudioManager Instance { get; private set; }

    private float masterVolume = 0.1f;
    private float musicVolume = 0.1f;
    private float sfxVolume = 0.1f;

    private void Start()
    {
        mixer.SetFloat("MasterVol", Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat("MusicVol", Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat("SFXVol", Mathf.Log10(sfxVolume) * 20);
        DontDestroyOnLoad(this.gameObject);
    }

    public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = value;
            mixer.SetFloat("MasterVol", Mathf.Log10(masterVolume) * 20);
        }
    }

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            mixer.SetFloat("MusicVol", Mathf.Log10(musicVolume) * 20);
        }
    }

    public float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            mixer.SetFloat("SFXVol",  Mathf.Log10(sfxVolume) * 20);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}
