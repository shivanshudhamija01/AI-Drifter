using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelServices : GenericSingleton<LevelServices>
{
    public EventController OnLevelCompleted;
    public EventController OnLevelRestarted;

    public LevelServices()
    {
        OnLevelCompleted = new EventController();
        OnLevelRestarted = new EventController();
    }
}
