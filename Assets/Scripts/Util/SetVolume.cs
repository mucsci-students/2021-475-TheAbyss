using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume", 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1.0f);
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1.0f);
    }

    public void SetMasterLevel(float sliderValue)
    {
        mixer.SetFloat("masterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("masterVolume", sliderValue);
    }

    public void SetSFXLevel(float sliderValue)
    {
        mixer.SetFloat("sfxVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("sfxVolume", sliderValue);
    }
    public void SetMusicLevel(float sliderValue)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("musicVolume", sliderValue);
    }
}
