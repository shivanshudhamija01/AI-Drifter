using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject worldSelectorPanel;
    [SerializeField] private GameObject levelSelectorPanel;
    [SerializeField] private GameObject desertLevels;
    [SerializeField] private GameObject iceLandLevels;
    [SerializeField] private GameObject cityWorldLevels;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Button[] buttons;

    void Awake()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(ButtonClicked);
        }
    }

    private void ButtonClicked()
    {
        AudioServices.Instance.PlayAudio.Invoke(Enums.Audios.guiClicked);
    }

    void OnEnable()
    {
        // Add Listeners to various event there 
        UIServices.Instance.PlayButtonPressed.AddListener(PlayButtonPressed);
        UIServices.Instance.SettingButtonPressed.AddListener(SettingButtonPressed);
        UIServices.Instance.OnDesertWorldSelected.AddListener(ShowDesertLevels);
        UIServices.Instance.OnCityWorldSelected.AddListener(ShowCityLevels);
        UIServices.Instance.OnIceWorldSelected.AddListener(ShowIceLandLevels);
        LevelServices.Instance.LoadLevel.AddListener(LoadLevel);
    }
    void OnDisable()
    {
        // Remove listeners 
        UIServices.Instance.PlayButtonPressed.RemoveListener(PlayButtonPressed);
        UIServices.Instance.SettingButtonPressed.RemoveListener(SettingButtonPressed);
        UIServices.Instance.OnDesertWorldSelected.RemoveListener(ShowDesertLevels);
        UIServices.Instance.OnCityWorldSelected.RemoveListener(ShowCityLevels);
        UIServices.Instance.OnIceWorldSelected.RemoveListener(ShowIceLandLevels);
        LevelServices.Instance.LoadLevel.RemoveListener(LoadLevel);
    }

    void PlayButtonPressed()
    {
        mainMenuPanel.SetActive(false);
        worldSelectorPanel.SetActive(true);
    }
    void SettingButtonPressed()
    {
        // Optional
        // mainMenuPanel.SetActive(false);
        settingPanel.SetActive(true);
    }
    void ShowDesertLevels()
    {
        worldSelectorPanel.SetActive(false);
        levelSelectorPanel.SetActive(true);
        desertLevels.SetActive(true);
        cityWorldLevels.SetActive(false);
        iceLandLevels.SetActive(false);
    }

    void ShowCityLevels()
    {
        worldSelectorPanel.SetActive(false);
        levelSelectorPanel.SetActive(true);
        desertLevels.SetActive(false);
        cityWorldLevels.SetActive(true);
        iceLandLevels.SetActive(false);
    }

    void ShowIceLandLevels()
    {
        worldSelectorPanel.SetActive(false);
        levelSelectorPanel.SetActive(true);
        desertLevels.SetActive(false);
        cityWorldLevels.SetActive(false);
        iceLandLevels.SetActive(true);
    }

    void LoadLevel(int level)
    {
        levelSelectorPanel.SetActive(false);
        // here enable the loading panel or timer screen to so that, player get enough time 
        // for playing the game , and all set 
        gamePlayPanel.SetActive(true);
    }
}
