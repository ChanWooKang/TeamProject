using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UI_SoundSetting : MonoBehaviour
{
    public AudioMixer m_masterMixer;
    
    [SerializeField] Slider m_sliderMaster;
    [SerializeField] Slider m_sliderBGM;
    [SerializeField] Slider m_sliderSFX;

    public void OpenUI()
    {
        gameObject.SetActive(true);

        m_masterMixer.GetFloat("Master", out float masterSlider);
        m_sliderMaster.value = masterSlider;
        m_masterMixer.GetFloat("BGM", out float bgmSlider);
        m_sliderBGM.value = bgmSlider;
        m_masterMixer.GetFloat("SFX", out float sfxSlider);
        m_sliderSFX.value = sfxSlider;

        Time.timeScale = 0f;
    }


    public void MasterVolume()
    {
        float sound = m_sliderMaster.value;

        if (sound == -40f)
            m_masterMixer.SetFloat("Master", -80f);
        else
            m_masterMixer.SetFloat("Master", sound);

    }
    public void BGMVolume()
    {
        float sound = m_sliderBGM.value;

        if (sound == -40f)
            m_masterMixer.SetFloat("BGM", -80f);
        else
            m_masterMixer.SetFloat("BGM", sound);
    }

    public void SFXVolume()
    {
        float sound = m_sliderSFX.value;

        if (sound == -40f)
            m_masterMixer.SetFloat("SFX", -80f);
        else
            m_masterMixer.SetFloat("SFX", sound);

    }

    public void ClickClose()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }


}
