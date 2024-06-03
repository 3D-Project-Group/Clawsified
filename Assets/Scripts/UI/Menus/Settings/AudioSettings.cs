using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
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
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        GameInfo.currentMusicVolume = volume;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }
    
    public void SetSoundFxVolume(float volume)
    {
        GameInfo.currentSoundFxVolume = volume;
        audioMixer.SetFloat("FXVolume", Mathf.Log10(volume) * 20);
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
        value += 0.01f;
        if (value > 1)
        {
            value = 1;
        }

        return value;
    }
    float LastSliderValue(float value)
    {
        value -= 0.01f;
        if (value < 0)
        {
            value = 0;
        }

        return value;
    }
}
