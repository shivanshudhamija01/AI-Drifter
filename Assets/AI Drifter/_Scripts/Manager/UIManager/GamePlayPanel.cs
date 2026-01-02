using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject gamePlayPopUp;


    void Awake()
    {
        pauseButton.onClick.AddListener(PauseButtonPressed);
    }
    void OnEnable()
    {
        UIServices.Instance.updateHealthBar.AddListener(ModifyHealthBar);
    }
    void OnDisable()
    {
        UIServices.Instance.updateHealthBar.RemoveListener(ModifyHealthBar);
    }

    void ModifyHealthBar(float health)
    {
        float fillAmount = health / 100f;
        healthBar.fillAmount = fillAmount;
    }
    void PauseButtonPressed()
    {
        UIServices.Instance.resetAudioOnPauseButton.Invoke();
        Time.timeScale = 0;
        gamePlayPopUp.SetActive(true);
    }
}
