using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IceLandLevelSelector : MonoBehaviour
{
    [SerializeField] private List<Button> levels;

    [SerializeField] private Button backButton;
    private const int IceLandStartIndex = 20;
    void Awake()
    {
        // Assign button callbacks only ONCE
        for (int i = 0; i < levels.Count; i++)
        {
            int index = i + IceLandStartIndex; // closure safety
            levels[i].onClick.RemoveAllListeners();
            levels[i].onClick.AddListener(() => LoadIthLevel(index));
        }

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(LoadWorldSelector);
    }

    void OnEnable()
    {
        // Only toggle interactable state when panel opens
        MakeInteractable();
    }

    void MakeInteractable()
    {
        int unlockedLevels = LevelProgressSaver.Instance.LoadData();

        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].interactable = ((i + IceLandStartIndex) < unlockedLevels);
        }
    }

    void LoadIthLevel(int level)
    {
        Debug.Log("Load the level number " + level);
        LevelServices.Instance.LoadLevel.Invoke(level);
    }

    void LoadWorldSelector()
    {
        UIServices.Instance.goBackToWorldSelection.Invoke();
    }
}
