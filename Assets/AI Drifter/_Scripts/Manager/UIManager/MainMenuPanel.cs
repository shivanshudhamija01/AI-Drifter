using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button shopButton;


    void Awake()
    {
        playButton.onClick.AddListener(PlayButtonPressed);
        settingButton.onClick.AddListener(SettingButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
        shopButton.onClick.AddListener(ShopButtonPressed);
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
        SceneManager.LoadScene("Shop");
        Debug.Log("Setting Button Pressed");
    }
}
