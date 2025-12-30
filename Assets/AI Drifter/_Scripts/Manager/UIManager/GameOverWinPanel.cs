using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverWinPanel : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private Animator animator;

    void Awake()
    {
        nextButton.onClick.AddListener(LoadNextLevel);
    }
    void LoadNextLevel()
    {
        LevelServices.Instance.LoadNextLevel.Invoke();
    }
    void OnEnable()
    {
        animator.SetTrigger("GameWin");
    }
}
