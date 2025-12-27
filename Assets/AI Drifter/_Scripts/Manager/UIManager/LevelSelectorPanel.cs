using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectorPanel : MonoBehaviour
{
    // First thing is that need to hold the reference of individual levels 
    // Hold the reference of these much button and after that , what i have to do is that 
    [SerializeField] private GameObject desertLevel;
    [SerializeField] private GameObject cityLevel;
    [SerializeField] private GameObject iceWorld;


    void OnEnable()
    {
        UIServices.Instance.OnDesertWorldSelected.AddListener(ShowDesertLevel);
        UIServices.Instance.OnCityWorldSelected.AddListener(ShowCityLevel);
        UIServices.Instance.OnIceWorldSelected.AddListener(ShowIceLevel);
    }
    void OnDisable()
    {
        UIServices.Instance.OnDesertWorldSelected.RemoveListener(ShowDesertLevel);
        UIServices.Instance.OnCityWorldSelected.RemoveListener(ShowCityLevel);
        UIServices.Instance.OnIceWorldSelected.RemoveListener(ShowIceLevel);
    }

    void ShowDesertLevel()
    {
        desertLevel.SetActive(true);
        cityLevel.SetActive(false);
        iceWorld.SetActive(false);
        Debug.Log("H");

    }
    void ShowCityLevel()
    {
        desertLevel.SetActive(false);
        cityLevel.SetActive(true);
        iceWorld.SetActive(false);
        Debug.Log("E");
    }
    void ShowIceLevel()
    {
        desertLevel.SetActive(false);
        cityLevel.SetActive(false);
        iceWorld.SetActive(true);
        Debug.Log("L");
    }
}
