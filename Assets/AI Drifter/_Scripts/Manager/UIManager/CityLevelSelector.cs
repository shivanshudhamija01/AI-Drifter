using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityLevelSelector : MonoBehaviour
{
    [SerializeField] private List<Button> levels;

    [SerializeField] private Button backButton;
    void Awake()
    {
        backButton.onClick.AddListener(LoadWorldSelector);
    }
    void OnEnable()
    {
        // call a method which make the level interactable 
        MakeInteractable();
    }


    void MakeInteractable()
    {
        int unlockedLevels = LevelProgressSaver.Instance.LoadData();
        for (int i = 10; i < levels.Count; i++)
        {
            if (i < unlockedLevels)
            {
                levels[i].interactable = true;

                int index = i; // avoid closure issue
                levels[i].onClick.AddListener(() => LoadIthLevel(index));
            }
            else
            {
                levels[i].interactable = false;
            }
        }
    }
    void LoadIthLevel(int level)
    {
        LevelServices.Instance.LoadLevel.Invoke(level);
    }

    private void LoadWorldSelector()
    {
        UIServices.Instance.goBackToWorldSelection.Invoke();
    }
}
