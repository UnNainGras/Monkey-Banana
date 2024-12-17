using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Button muteButton;
    public Sprite volumeOnSprite;
    public Sprite volumeOffSprite;

    private void Start()
    {
        if (AudioManager.instance != null)
        {
            volumeSlider.value = AudioManager.instance.GetVolume();
            UpdateMuteButtonIcon();
        }

        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        muteButton.onClick.AddListener(OnMuteButtonClicked);
    }

    private void OnSliderValueChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(value);
            if (AudioManager.instance.IsMuted() && value > 0f)
            {
                AudioManager.instance.ToggleMute();
                UpdateMuteButtonIcon();
            }
        }
    }

    private void OnMuteButtonClicked()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ToggleMute();
            UpdateMuteButtonIcon();
        }
    }

    private void UpdateMuteButtonIcon()
    {
        if (AudioManager.instance != null)
        {
            bool isMuted = AudioManager.instance.IsMuted();
            muteButton.image.sprite = isMuted ? volumeOffSprite : volumeOnSprite;
            volumeSlider.interactable = !isMuted;
        }
    }
}
