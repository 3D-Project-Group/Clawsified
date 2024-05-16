using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown windowModeDropdown;
    [SerializeField] private TMP_Dropdown fpsLimitDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions;

    void Start()
    {
        //Resolution Control Start
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();

        float currentRefreshRateRatio = (float)Screen.currentResolution.refreshRateRatio.value;

        foreach(Resolution resolution in resolutions)
        {
            if (resolution.refreshRateRatio.value == currentRefreshRateRatio)
            {
                filteredResolutions.Add(resolution);
            }
        }

        List<string> options = new List<string>();

        foreach(Resolution resolution in filteredResolutions)
        {
            string resolutionOption = $"{resolution.width}x{resolution.height} {resolution.refreshRateRatio.value}Hz";
            options.Add(resolutionOption);
            if (resolution.width == Screen.width && resolution.height == Screen.height && GameInfo.currentResolutionIndex == -1)
            {
                GameInfo.currentResolutionIndex = filteredResolutions.IndexOf(resolution);
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GameInfo.currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //Fps limit dropdown Update
        fpsLimitDropdown.value = GameInfo.currentFpsLimitIndex;

        //Window Mode dropdown Update
        if (GameInfo.fullScreen)
            windowModeDropdown.value = 0;
        else
            windowModeDropdown.value = 1;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        GameInfo.currentResolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, GameInfo.fullScreen);
    }

    public void SetFpsLimit(int fpsLimitIndex)
    {
        GameInfo.currentFpsLimitIndex = fpsLimitIndex;
        switch (fpsLimitIndex)
        {
            case 0:
                Application.targetFrameRate = 30;
                break;
            case 1:
                Application.targetFrameRate = 60;
                break;
            case 2:
                Application.targetFrameRate = 120;
                break;
            case 3:
                Application.targetFrameRate = -1;
                break;
        }
    }

    public void SetWindowMode(int windowModeIndex)
    {
        if(windowModeIndex == 0)
        {
            Screen.fullScreen = true;
            GameInfo.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
            GameInfo.fullScreen = false;
        }
    }
}
