using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public GameObject settingsMenu;
    public GameObject mainMenu;
    public GameObject titleScreen;
    public SpawnCrow spawnCrow;
    public Toggle fullScreenToggle;
    public Scrollbar volumeSlider;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i; 
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        

        SetVolume(PlayerPrefs.GetFloat("Volume", 0.5f));
        SetFullScreen(PlayerPrefs.GetInt("FullScreen", 1) == 1);
        SetResolution(PlayerPrefs.GetInt("Resolution", 0));

        fullScreenToggle.isOn = Screen.fullScreen;
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0.5f);
    }
    public void SetVolume(float volume)
    {
        AudioManager.instance.SetVolume(volume);
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    public void MainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
        spawnCrow.AbleToSpawn();
    }

    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
        spawnCrow.Stop();
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
