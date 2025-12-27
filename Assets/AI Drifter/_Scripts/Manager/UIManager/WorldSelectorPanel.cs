using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSelectorPanel : MonoBehaviour
{
    [SerializeField] private Button desertWorld;
    [SerializeField] private Button cityWorld;
    [SerializeField] private Button iceWorld;

    void Awake()
    {
        ModifyWorldInteractablity();
        desertWorld.onClick.AddListener(DesertWorldSelected);
        cityWorld.onClick.AddListener(CityWorldSelected);
        iceWorld.onClick.AddListener(IceWorldSelected);
        // Update the world interactablilty according to the data stored in the player pref
    }

    void DesertWorldSelected()
    {
        UIServices.Instance.OnDesertWorldSelected.Invoke();
        Debug.Log("Desert world is clicked");
    }
    void CityWorldSelected()
    {
        UIServices.Instance.OnCityWorldSelected.Invoke();
        Debug.Log("City world is selected");
    }
    void IceWorldSelected()
    {
        UIServices.Instance.OnIceWorldSelected.Invoke();
        Debug.Log("Ice World is selected");
    }

    void ModifyWorldInteractablity()
    {
        // Get the level number from the player pref and after that 
        // modify the interactablity accordingly, 
        // desert if level is less than 10
        // city if level is less than 20
        // ice if level is less than 30 

        int totalCompletedLevel = LevelProgressSaver.Instance.LoadData();
        if (totalCompletedLevel < 10)
        {
            desertWorld.interactable = true;
            cityWorld.interactable = false;
            iceWorld.interactable = false;
        }
        else if (totalCompletedLevel < 20)
        {
            desertWorld.interactable = true;
            cityWorld.interactable = true;
            iceWorld.interactable = false;
        }
        else if (totalCompletedLevel < 30)
        {
            desertWorld.interactable = true;
            cityWorld.interactable = false;
            iceWorld.interactable = false;
        }
    }
}
