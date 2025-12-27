using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesertLevelSelector : MonoBehaviour
{
    // In a list store all the button 
    // After store in a awake call a method 
    // That will traverse accordingly and unlock or make the button interactable

    [SerializeField] private List<Button> levels;

    void OnEnable()
    {
        // call a method which make the level interactable 
        MakeInteractable();
    }


    void MakeInteractable()
    {
        int unlockedLevels = LevelProgressSaver.Instance.LoadData();
        for (int i = 0; i < levels.Count; i++)
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
        Debug.Log("Load the level number " + level);
        LevelServices.Instance.LoadLevel.Invoke(level);
    }
}
