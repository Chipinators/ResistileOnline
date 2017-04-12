using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    public InputField usernameField;
    public Slider volumeSlider;
    public Dropdown resolutionDropdown;
    public Resolution[] resList;
    public bool nameChange = false;
    public string username;
    public Toggle fullscreenToggle;
    private bool isFullscreen;
    private bool resChange = true, fullscreenChange = false;

    void Start()
    {
        usernameField.transform.FindChild("Placeholder").GetComponent<Text>().text = NetworkManager.networkManager.username;    //Set Username
        volumeSlider.GetComponent<Slider>().value = AudioManager.audioManager.GetComponent<AudioSource>().volume;   //Set Volume Level
        resolutionDropdown.interactable = !isFullscreen;
        isFullscreen = Screen.fullScreen;
        fillResolutions();
        Resolution curRes = Screen.currentResolution;
        if (!curRes.Equals(resList[0])) resChange = true;
        if (Screen.fullScreen) fullscreenToggle.isOn = true;    //Set Fullscreen
    }

    public void ApplySettings()
    {
        if (nameChange) NetworkManager.networkManager.username = usernameField.transform.FindChild("Text").GetComponent<Text>().text;
        if (resChange) setResolution();
        if (fullscreenChange) setFullScreen();
    }

    public void setVolume(float value)
    {
        AudioManager.audioManager.GetComponent<AudioSource>().volume = volumeSlider.GetComponent<Slider>().value;
    }

    public void setResolution()
    {
        if (!isFullscreen)
        {
            resolutionDropdown.interactable = !isFullscreen;
        }
        Resolution setRes = resList[resolutionDropdown.value];
        Screen.SetResolution(setRes.width, setRes.height, false);
        
    }

    public void setFullScreen()
    {
        isFullscreen = !isFullscreen;
        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else
        {
            setResolution();
        }
    }
    

    private void fillResolutions()
    {
        resList = new Resolution[4];
        Resolution tmp = new Resolution();
        tmp.width = 960;
        tmp.height = 540;
        resList[0] = tmp;
        tmp.width = 1280;
        tmp.height = 720;
        resList[1] = tmp;
        tmp.width = 1600;
        tmp.height = 900;
        resList[2] = tmp;
        tmp.width = 1920;
        tmp.height = 1080;
        resList[3] = tmp;
    }


    public void nameChanged()
    {
        nameChange = true;
    }
    public void resolutionChanged()
    {
        resChange = true;
    }
    public void fullscreenChanged()
    {
        fullscreenChange = true;
        resolutionDropdown.interactable = !isFullscreen;
    }
}
