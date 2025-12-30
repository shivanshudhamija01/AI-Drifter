using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverLosePanel : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Animator animator;
    [SerializeField] private Button homeButton;


    void Awake()
    {
        restartButton.onClick.AddListener(LevelRestart);
    }
    void LevelRestart()
    {
        LevelServices.Instance.LevelRestart.Invoke();
    }
    void OnEnable()
    {
        animator.SetTrigger("GameLost");
    }
}
