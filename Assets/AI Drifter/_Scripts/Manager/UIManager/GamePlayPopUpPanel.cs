using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPopUpPanel : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button homeButton;

    void Awake()
    {
        exitButton.onClick.AddListener(OpenGameScene);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        homeButton.onClick.AddListener(OnHomeButtonClicked);
    }

    void OpenGameScene()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    void OnRestartButtonClicked()
    {
        // On Restart button clicked, just disable this panel and make a call to restart the level 
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        LevelServices.Instance.LevelRestart.Invoke();
    }
    void OnHomeButtonClicked()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        // On home button clicked what i want is that , reset the level data 
        // and disable this panel and also disable the parent panel and enable the main menu panel
        LevelServices.Instance.ResetLevel.Invoke();
        UIServices.Instance.onHomeButtonClicked.Invoke();
    }
}
