using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameOverWinPanel : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI totalCoins;

    void Awake()
    {
        nextButton.onClick.AddListener(LoadNextLevel);
        homeButton.onClick.AddListener(BackToMain);
    }
    void LoadNextLevel()
    {
        LevelServices.Instance.LoadNextLevel.Invoke();
    }
    void OnEnable()
    {
        animator.SetTrigger("GameWin");
        int updatedTotalCoins = LevelProgressSaver.Instance.GetCoin();
        totalCoins.text = updatedTotalCoins.ToString();
    }
    void BackToMain()
    {
        this.gameObject.SetActive(false);
        LevelServices.Instance.ResetLevel.Invoke();
        UIServices.Instance.onHomeButtonClicked.Invoke();

    }
}
