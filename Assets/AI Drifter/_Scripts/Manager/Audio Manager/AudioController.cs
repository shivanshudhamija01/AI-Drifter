using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioController : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public AudioSource backgroundAudioSource;
    public AudioSource sfxAudioSource;

    private const string BGM_VOLUME = "bgm";
    private const string SFX_VOLUME = "sfx";

    void Start()
    {
        if (bgmSlider != null && backgroundAudioSource != null)
        {
            bgmSlider.value = backgroundAudioSource.volume;
        }

        if (sfxSlider != null && sfxAudioSource != null)
        {
            sfxSlider.value = sfxAudioSource.volume;
        }

        if (!PlayerPrefs.HasKey(BGM_VOLUME))
        {
            PlayerPrefs.SetFloat(BGM_VOLUME, 1f);
        }
        else
        {
            backgroundAudioSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME, 1f);
            bgmSlider.value = backgroundAudioSource.volume;
        }
        if (!PlayerPrefs.HasKey(SFX_VOLUME))
        {
            PlayerPrefs.SetFloat(SFX_VOLUME, 1f);
        }
        else
        {
            sfxAudioSource.volume = PlayerPrefs.GetFloat(SFX_VOLUME, 1f);
            sfxSlider.value = sfxAudioSource.volume;
        }
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(SetVolumeBGM);
        }
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetVolumeSFX);
        }

    }

    // This method will be called when the slider's value changes
    private void SetVolumeBGM(float volume)
    {
        if (backgroundAudioSource != null)
        {
            backgroundAudioSource.volume = volume;
            PlayerPrefs.SetFloat(BGM_VOLUME, volume);
        }
    }
    private void SetVolumeSFX(float volume)
    {
        if (sfxAudioSource != null)
        {
            sfxAudioSource.volume = volume;
            PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        }
    }
}
