using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.coin;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Coin Collected");
        Destroy(gameObject);
    }
}
