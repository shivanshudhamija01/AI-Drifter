using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour, ICollectible
{
    public Enums.Collectibles GetCollectibleType()
    {
        return Enums.Collectibles.timer;
    }

    public void OnCollected(GameObject collector)
    {
        Debug.Log("Timer Collected");
        Destroy(gameObject);
    }
}
