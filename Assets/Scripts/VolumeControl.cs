using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public Button muteButton;
    public Sprite volumeOnSprite;
    public Sprite volumeOffSprite;

    private bool isInteractingWithSlider = false; // Ajout�

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

    private void Update()
    {
        // V�rifie si l'utilisateur a rel�ch� le slider
        if (isInteractingWithSlider && !Input.GetMouseButton(0))
        {
            isInteractingWithSlider = false;
            UpdateMuteButtonIcon(); // Met � jour l'ic�ne apr�s interaction
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (AudioManager.instance != null)
        {
            isInteractingWithSlider = true;
            AudioManager.instance.SetVolume(value);
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
        if (AudioManager.instance != null && !isInteractingWithSlider) 
        {
            bool isMuted = AudioManager.instance.IsMuted();
            muteButton.image.sprite = isMuted ? volumeOffSprite : volumeOnSprite;
        }
    }
}
