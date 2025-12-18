using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.shield;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Shield Collected");
        Destroy(gameObject);
    }
}
