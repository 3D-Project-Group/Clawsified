using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettings : MonoBehaviour
{
    [Header("Screen Size Control")]
    [SerializeField] private Button fullScreenButton;
    [SerializeField] private Button windowedButton;
    
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
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
            string resolutionOption = $"{resolution.width}x{resolution.height}";
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
        
        //Change Buttons Colors
        if (GameInfo.fullScreen)
        {
            fullScreenButton.interactable = false;
            windowedButton.interactable = true;
            
            //Set Colors to the texts
            fullScreenButton.gameObject.GetComponentInChildren<TMP_Text>().color = Color.white;
            windowedButton.gameObject.GetComponentInChildren<TMP_Text>().color = Color.black;
        }
        else
        {
            fullScreenButton.interactable = true;
            windowedButton.interactable = false;
            
            //Set Colors to the texts
            fullScreenButton.gameObject.GetComponentInChildren<TMP_Text>().color = Color.black;
            windowedButton.gameObject.GetComponentInChildren<TMP_Text>().color = Color.white;
        }
    }
    void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        GameInfo.currentResolutionIndex = resolutionIndex;
        Screen.SetResolution(resolution.width, resolution.height, GameInfo.fullScreen);
    }

    void SetFpsLimit(int fpsLimitIndex)
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

    void SetWindowMode(int windowModeIndex)
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

    public void ActivateButton(Button btn)
    {
        btn.interactable = true;
        btn.GetComponentInChildren<TMP_Text>().color = Color.black;
    }
    public void DeactivateButton(Button btn)
    {
        btn.interactable = false;
        btn.GetComponentInChildren<TMP_Text>().color = Color.white;
    }

    //Resolution
    public void NextResolution()
    {
        resolutionDropdown.value = NextDropdownValue(resolutionDropdown.value, resolutionDropdown.options.Count - 1);
        SetResolution(resolutionDropdown.value);
    }
    
    public void LastResolution()
    {
        resolutionDropdown.value = LastDropdownValue(resolutionDropdown.value, resolutionDropdown.options.Count - 1);
        SetResolution(resolutionDropdown.value);
    }
    
    //Fps
    public void NextFps()
    {
        fpsLimitDropdown.value = NextDropdownValue(fpsLimitDropdown.value, fpsLimitDropdown.options.Count - 1);
        SetFpsLimit(fpsLimitDropdown.value);
    }
    
    public void LastFps()
    {
        fpsLimitDropdown.value = NextDropdownValue(fpsLimitDropdown.value, fpsLimitDropdown.options.Count - 1);
        SetFpsLimit(fpsLimitDropdown.value);
    }
    
    //Graphics
    public void NextGraphic()
    {
        graphicsDropdown.value = NextDropdownValue(graphicsDropdown.value, graphicsDropdown.options.Count - 1);
        //SetGraphics(graphicsDropdown.value);
    }
    
    public void LastGraphic()
    {
        graphicsDropdown.value = NextDropdownValue(graphicsDropdown.value, graphicsDropdown.options.Count - 1);
        //SetGraphics(graphicsDropdown.value);
    }
    
    //Auxiliary Functions
    int NextDropdownValue(int value, int maxValue)
    {
        value++;
        if (value > maxValue)
        {
            value = 0;
        }

        return value;
    }
    int LastDropdownValue(int value, int maxValue)
    {
        value--;
        if (value < 0)
        {
            value = maxValue;
        }

        return value;
    }
}
