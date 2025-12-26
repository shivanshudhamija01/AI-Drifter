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
        // Get the level number from the player pref
        // then traverse the list and make the button interactable accodingly

        for (int i = 0; i < levels.Count; i++)
        {
            if (i < 5)
            {
                levels[i].interactable = true;
            }
            else
            {
                levels[i].interactable = false;
            }
        }
    }
}
