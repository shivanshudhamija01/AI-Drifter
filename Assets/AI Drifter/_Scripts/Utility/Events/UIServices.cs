using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIServices : GenericSingleton<UIServices>
{
    #region  MainMenuPane;
    public EventController PlayButtonPressed;
    public EventController SettingButtonPressed;
    public EventController ShopButtonPressed;
    #endregion

    #region WorldSelectorPanel
    public EventController OnDesertWorldSelected;
    public EventController OnCityWorldSelected;
    public EventController OnIceWorldSelected;
    #endregion

    public EventController goBackToWorldSelection;
    public EventController<float> updateHealthBar;

    public UIServices()
    {
        PlayButtonPressed = new EventController();
        SettingButtonPressed = new EventController();
        ShopButtonPressed = new EventController();
        OnDesertWorldSelected = new EventController();
        OnCityWorldSelected = new EventController();
        OnIceWorldSelected = new EventController();
        goBackToWorldSelection = new EventController();
        updateHealthBar = new EventController<float>();
    }
}
