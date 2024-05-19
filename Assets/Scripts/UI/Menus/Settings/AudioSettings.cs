using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundFxVolumeSlider;

    void Start()
    {
        masterVolumeSlider.value = GameInfo.currentMasterVolume;
        musicVolumeSlider.value = GameInfo.currentMusicVolume;
        soundFxVolumeSlider.value = GameInfo.currentSoundFxVolume;
    }

    public void SetMasterVolume(float volume)
    {
        GameInfo.currentMasterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        GameInfo.currentMusicVolume = volume;
    }
    
    public void SetSoundFxVolume(float volume)
    {
        GameInfo.currentSoundFxVolume = volume;
    }

    //For buttons
    public void IncreaseSlider(Slider selectedSlider)
    {
        selectedSlider.value = NextSliderValue(selectedSlider.value);
    }
    
    public void DecreaseSlider(Slider selectedSlider)
    {
        selectedSlider.value = LastSliderValue(selectedSlider.value);
    }
    
    float NextSliderValue(float value)
    {
        value++;
        if (value > 100)
        {
            value = 100;
        }

        return value;
    }
    float LastSliderValue(float value)
    {
        value--;
        if (value < 0)
        {
            value = 0;
        }

        return value;
    }
}
