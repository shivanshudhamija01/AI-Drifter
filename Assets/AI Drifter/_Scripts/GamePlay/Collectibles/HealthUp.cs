using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.healthUp;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Health Collected");
        Destroy(gameObject);
    }
}
