using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    void Start()
    {
        masterVolumeSlider.value = GameInfo.currentMasterVolume;
        musicVolumeSlider.value = GameInfo.currentMusicVolume;
    }

    public void SetMasterVolume(float volume)
    {
        print("Master: " + GameInfo.currentMasterVolume);
        GameInfo.currentMasterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        print("Music: " + GameInfo.currentMusicVolume);
        GameInfo.currentMusicVolume = volume;
    }
}