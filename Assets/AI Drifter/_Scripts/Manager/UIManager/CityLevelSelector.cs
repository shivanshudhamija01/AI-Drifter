using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityLevelSelector : MonoBehaviour
{
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
