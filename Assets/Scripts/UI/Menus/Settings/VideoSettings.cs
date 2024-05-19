using System.Collections.Generic;
using System.Linq;
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
        #region Resolution Control
        // Do not change this part plsss, I created this region to say that without it, nothing works on the resolution part :)
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();

        //Filter Resolutions According to the refreshing ratio of the monitor
        float currentRefreshRateRatio = (float)Screen.currentResolution.refreshRateRatio.value;
        foreach(Resolution resolution in resolutions)
        {
            if (resolution.refreshRateRatio.value == currentRefreshRateRatio)
            {
                filteredResolutions.Add(resolution);
            }
        }
        //Update the options of screen size
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
        //Add options and refresh to show the current
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = GameInfo.currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        #endregion
        
        //Graphics Level
        graphicsDropdown.AddOptions(QualitySettings.names.ToList());
        graphicsDropdown.value = QualitySettings.GetQualityLevel();

        //Fps limit dropdown Update
        fpsLimitDropdown.value = GameInfo.currentFpsLimitIndex;
        
        //Change Buttons Colors according the the mode selected
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
    void SetGraphic(int graphicIndex)
    {
        QualitySettings.SetQualityLevel(graphicIndex);
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
        SetGraphic(graphicsDropdown.value);
    }
    
    public void LastGraphic()
    {
        graphicsDropdown.value = LastDropdownValue(graphicsDropdown.value, graphicsDropdown.options.Count - 1);
        SetGraphic(graphicsDropdown.value);
    }
    
    //Auxiliary Functions
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
