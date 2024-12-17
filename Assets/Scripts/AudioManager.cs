using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private bool isMuted = false;
    private float currentVolume = 1f; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadAudioSettings();
        UpdateAudioSettings();
    }

    public void SetVolume(float volume)
    {
        currentVolume = volume;
        UpdateAudioSettings();
        SaveAudioSettings();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        UpdateAudioSettings();
        SaveAudioSettings();
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public float GetVolume()
    {
        return currentVolume;
    }

    private void UpdateAudioSettings()
    {
        AudioListener.volume = isMuted ? 0f : currentVolume;
    }

    private void SaveAudioSettings()
    {
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.SetFloat("Volume", currentVolume);
        PlayerPrefs.Save();
    }

    private void LoadAudioSettings()
    {
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        currentVolume = PlayerPrefs.GetFloat("Volume", 1f);
    }
}
