using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private TextMeshProUGUI coinCount;


    void Awake()
    {
        playButton.onClick.AddListener(PlayButtonPressed);
        settingButton.onClick.AddListener(SettingButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
        shopButton.onClick.AddListener(ShopButtonPressed);
    }
    void OnEnable()
    {
        int availableCoins = 0;
        availableCoins = LevelProgressSaver.Instance.GetCoin();
        coinCount.text = availableCoins.ToString();
    }
    void PlayButtonPressed()
    {
        UIServices.Instance.PlayButtonPressed.Invoke();
    }
    void SettingButtonPressed()
    {
        UIServices.Instance.SettingButtonPressed.Invoke();
    }
    void QuitButtonPressed()
    {
        Application.Quit();
    }
    void ShopButtonPressed()
    {
        // SceneManager.LoadScene("Shop");
        // Debug.Log("Setting Button Pressed");
    }
}
