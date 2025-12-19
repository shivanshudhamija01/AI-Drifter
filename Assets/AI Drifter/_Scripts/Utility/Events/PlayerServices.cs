using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerServices : GenericSingleton<PlayerServices>
{
    public EventController<Enums.Collectibles> OnCollectiblePicked;

    public PlayerServices()
    {
        OnCollectiblePicked = new EventController<Enums.Collectibles>(); 
    }
}

