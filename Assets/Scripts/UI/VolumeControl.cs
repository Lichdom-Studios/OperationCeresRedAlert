using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float valueMultiplier = 30f;
    [SerializeField] Toggle muteToggle;

    void Start()
    {
        slider.onValueChanged.AddListener(SliderValueChanged);
        muteToggle.onValueChanged.AddListener(MuteToggleChanged);        

        if(volumeParameter == "MasterVolume")
            slider.value = PlayerPrefs.GetFloat(volumeParameter, 0.5f);
        else
            slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.maxValue);
    }

    private void MuteToggleChanged(bool enableSound)
    {
        if (enableSound)
        {
            float newValue = PlayerPrefs.GetFloat(volumeParameter, slider.maxValue);

            slider.value = newValue == slider.minValue ? 0.25f : newValue;
        }
        else
            slider.value = slider.minValue;
    }

    private void SliderValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, value:Mathf.Log10(value)*valueMultiplier);
        muteToggle.isOn = slider.value > slider.minValue;
    }

    void Update()
    {
        
    }

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }

    private void OnDisable()
    {
        SaveVolume();
    }
}
