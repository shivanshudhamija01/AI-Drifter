using UnityEngine;

public class PlayerServices : GenericSingleton<PlayerServices>
{
    public EventController<Enums.Collectibles> OnCollectiblePicked;
    public EventController<GameObject> OnCoinPickedUp;
    public EventController OnGhostCollected;
    public EventController OnPowerAttack;
    public EventController OnPlayerDead;
    public EventController<int> UpdatePlayerHealth;

    public PlayerServices()
    {
        OnCollectiblePicked = new EventController<Enums.Collectibles>();
        OnCoinPickedUp = new EventController<GameObject>();
        OnGhostCollected = new EventController();
        OnPowerAttack = new EventController();
        OnPlayerDead = new EventController();
        UpdatePlayerHealth = new EventController<int>();
    }
}

