using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Image healthBar;


    void Awake()
    {
        pauseButton.onClick.AddListener(PauseButtonPressed);
    }

    void PauseButtonPressed()
    {
        Time.timeScale = 0;
    }
}
