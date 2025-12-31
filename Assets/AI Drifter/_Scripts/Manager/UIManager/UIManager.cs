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
    [SerializeField] private GameObject gameLostPanel;
    [SerializeField] private GameObject gameWinPanel;
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
        UIServices.Instance.goBackToWorldSelection.AddListener(ShowWorldMap);
        PlayerServices.Instance.OnPlayerDead.AddListener(LoadGameLostPanel);
        LevelServices.Instance.OnLevelCompleted.AddListener(LoadGameWinPanel);
        LevelServices.Instance.LoadLevel.AddListener(LoadLevel);
        // LevelServices.Instance.OnLevelRestarted.AddListener(LevelRestarted);
        LevelServices.Instance.LevelRestart.AddListener(LevelRestarted);
        LevelServices.Instance.LoadNextLevel.AddListener(LoadNextLevel);
        UIServices.Instance.goBackToMainFromWorld.AddListener(OpenUpMainFromWorld);
    }
    void OnDisable()
    {
        // Remove listeners 
        UIServices.Instance.PlayButtonPressed.RemoveListener(PlayButtonPressed);
        UIServices.Instance.SettingButtonPressed.RemoveListener(SettingButtonPressed);
        UIServices.Instance.OnDesertWorldSelected.RemoveListener(ShowDesertLevels);
        UIServices.Instance.OnCityWorldSelected.RemoveListener(ShowCityLevels);
        UIServices.Instance.OnIceWorldSelected.RemoveListener(ShowIceLandLevels);
        UIServices.Instance.goBackToWorldSelection.RemoveListener(ShowWorldMap);
        PlayerServices.Instance.OnPlayerDead.RemoveListener(LoadGameLostPanel);
        LevelServices.Instance.LoadLevel.RemoveListener(LoadLevel);
        LevelServices.Instance.OnLevelCompleted.RemoveListener(LoadGameWinPanel);
        // LevelServices.Instance.OnLevelRestarted.RemoveListener(LevelRestarted);
        LevelServices.Instance.LevelRestart.RemoveListener(LevelRestarted);
        LevelServices.Instance.LoadNextLevel.RemoveListener(LoadNextLevel);
        UIServices.Instance.goBackToMainFromWorld.RemoveListener(OpenUpMainFromWorld);
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
    void ShowWorldMap()
    {
        worldSelectorPanel.SetActive(true);
        levelSelectorPanel.SetActive(false);
        desertLevels.SetActive(false);
        cityWorldLevels.SetActive(false);
        iceLandLevels.SetActive(false);
    }
    void LoadLevel(int level)
    {
        levelSelectorPanel.SetActive(false);
        // here enable the loading panel or timer screen to so that, player get enough time 
        // for playing the game , and all set 
        gamePlayPanel.SetActive(true);
    }
    void LoadGameWinPanel()
    {
        StartCoroutine(LoadLevelWinPanelAfterDelay());
    }
    IEnumerator LoadLevelWinPanelAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        gamePlayPanel.SetActive(false);
        gameWinPanel.SetActive(true);

    }
    void LoadGameLostPanel()
    {
        StartCoroutine(LoadLostPanelAfterADelay());
    }
    IEnumerator LoadLostPanelAfterADelay()
    {
        yield return new WaitForSeconds(3f);
        gamePlayPanel.SetActive(false);
        gameLostPanel.SetActive(true);
    }
    void LevelRestarted()
    {
        StopAllCoroutines();
        Debug.Log("Level Restart Kro ");
        gameLostPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }
    void LoadNextLevel()
    {
        gamePlayPanel.SetActive(true);
        gameWinPanel.SetActive(false);
    }

    void OpenUpMainFromWorld()
    {
        worldSelectorPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    // On level completed and load next level are two different event remember it 

}
