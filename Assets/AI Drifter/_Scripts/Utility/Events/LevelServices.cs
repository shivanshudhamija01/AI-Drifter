using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelServices : GenericSingleton<LevelServices>
{
    public EventController OnLevelCompleted;
    public EventController OnLevelRestarted;
    public EventController<int> LoadLevel;

    public LevelServices()
    {
        OnLevelCompleted = new EventController();
        OnLevelRestarted = new EventController();
        LoadLevel = new EventController<int>();
    }
}
