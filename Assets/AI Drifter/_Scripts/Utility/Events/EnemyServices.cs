using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyServices : GenericSingleton<EnemyServices>
{
    public EventController<GameObject> OnObstacleHit;

    public EnemyServices()
    {
        OnObstacleHit = new EventController<GameObject>();
    }
}
