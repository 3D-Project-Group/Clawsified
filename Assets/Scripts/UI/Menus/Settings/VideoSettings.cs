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
        // Ensure to retain this region for the resolution functionality
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        resolutionDropdown.ClearOptions();

        // Filter Resolutions According to the current refresh rate of the monitor
        float currentRefreshRateRatio = (float)Screen.currentResolution.refreshRateRatio.value;
        foreach (Resolution resolution in resolutions)
        {
            if ((float)resolution.refreshRateRatio.value == currentRefreshRateRatio)
            {
                filteredResolutions.Add(resolution);
            }
        }

        // Check if filteredResolutions is empty, fallback to default resolutions if needed
        if (filteredResolutions.Count == 0)
        {
            // Optionally log a warning if no resolutions match the refresh rate ratio
            Debug.LogWarning("No resolutions found matching the current refresh rate ratio. Using all available resolutions.");
            filteredResolutions.AddRange(resolutions);
        }

        // Update the options of screen size
        List<string> options = new List<string>();
        int currentResolutionIndex = -1;
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            Resolution resolution = filteredResolutions[i];
            string resolutionOption = $"{resolution.width}x{resolution.height}";
            options.Add(resolutionOption);

            if (resolution.width == Screen.width && resolution.height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Check if currentResolutionIndex was set, if not, set it to a default value
        if (currentResolutionIndex == -1)
        {
            currentResolutionIndex = filteredResolutions.Count - 1;
            Debug.LogWarning("Current resolution not found in the list. Defaulting to the first resolution.");
        }

        // Add options and refresh to show the current value
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
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
    public void SetResolution(int resolutionIndex)
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

    public void SetWindowMode(int windowModeIndex)
    {
        if(windowModeIndex == 0)
        {
            Screen.fullScreen = true;
            GameInfo.fullScreen = true;
            Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, GameInfo.fullScreen);
        }
        else
        {
            Screen.fullScreen = false;
            GameInfo.fullScreen = false;
            Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, GameInfo.fullScreen);
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
        fpsLimitDropdown.value = LastDropdownValue(fpsLimitDropdown.value, fpsLimitDropdown.options.Count - 1);
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
