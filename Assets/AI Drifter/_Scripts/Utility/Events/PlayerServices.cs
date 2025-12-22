using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class PlayerServices : GenericSingleton<PlayerServices>
{
    public EventController<Enums.Collectibles> OnCollectiblePicked;
    public EventController<GameObject> OnCoinPickedUp;

    public PlayerServices()
    {
        OnCollectiblePicked = new EventController<Enums.Collectibles>();
        OnCoinPickedUp = new EventController<GameObject>();
    }
}

