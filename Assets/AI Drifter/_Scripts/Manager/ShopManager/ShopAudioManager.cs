using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip guiClick;
    [SerializeField] private AudioClip negativeGuiClick;
    void OnEnable()
    {
        ShopUIManager.OnButtonClicked += ButtonClicked;
        ShopUIManager.OnSelectedButtonClicked += SelectedButtonClicked;
    }
    void OnDisable()
    {
        ShopUIManager.OnButtonClicked -= ButtonClicked;
    }

    void ButtonClicked()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = guiClick;
        audioSource.Play();
    }
    void SelectedButtonClicked()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = negativeGuiClick;
        audioSource.Play();
    }
}
