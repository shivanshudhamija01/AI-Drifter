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
        desertWorld.onClick.AddListener(DesertWorldSelected);
        cityWorld.onClick.AddListener(CityWorldSelected);
        iceWorld.onClick.AddListener(IceWorldSelected);

        // Update the world interactablilty according to the data stored in the player pref
    }

    void DesertWorldSelected()
    {
        UIServices.Instance.OnDesertWorldSelected.Invoke();
    }
    void CityWorldSelected()
    {
        UIServices.Instance.OnCityWorldSelected.Invoke();
    }
    void IceWorldSelected()
    {
        UIServices.Instance.OnIceWorldSelected.Invoke();
    }

    void ModifyWorldInteractablity()
    {
        // Get the level number from the player pref and after that 
        // modify the interactablity accordingly, 
        // desert if level is less than 10
        // city if level is less than 20
        // ice if level is less than 30 
    }
}
