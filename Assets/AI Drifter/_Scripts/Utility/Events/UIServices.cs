using System.Collections;
using System.Collections.Generic;
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
}
