using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip guiClick;
    [SerializeField] private AudioClip negativeGuiClick;
    [SerializeField] private AudioClip purchaseSuccess;
    [SerializeField] private AudioClip purchaseFail;
    void OnEnable()
    {
        ShopUIManager.OnButtonClicked += ButtonClicked;
        ShopUIManager.OnSelectedButtonClicked += SelectedButtonClicked;
        CarManager.OnPurchaseSuccessed += PurchaseSuccessed;
        CarManager.OnPurchaseFail += PurchaseFailed;
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
    void PurchaseSuccessed()
    {
        audioSource.Stop();
        audioSource.clip = purchaseSuccess;
        audioSource.Play();
    }
    void PurchaseFailed()
    {
        audioSource.Stop();
        audioSource.clip = purchaseFail;
        audioSource.Play();
    }
}
